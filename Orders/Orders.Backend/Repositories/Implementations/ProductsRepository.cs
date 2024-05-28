using Microsoft.EntityFrameworkCore;
using Orders.Backend.Data;
using Orders.Backend.Helpers;
using Orders.Backend.Repositories.Interfaces;
using Orders.Shared.DTOs;
using Orders.Shared.Entities;
using Orders.Shared.Responses;

namespace Orders.Backend.Repositories.Implementations
{
    public class ProductsRepository : GenericRepository<Product>, IProductsRepository
    {
        private readonly DataContext _context;
        private readonly IFileStoragecs _fileStoragecs;

        public ProductsRepository(DataContext context, IFileStoragecs fileStoragecs):base(context)
        {
           _context = context;
           _fileStoragecs = fileStoragecs;
        }
        public  async Task<ActionResponse<Product>> AddFullAsync(ProductDTO productDTO)
        {
            try
            {
                var newProduct = new Product
                {
                    Name = productDTO.Name,
                    Description = productDTO.Description,
                    Price = productDTO.Price,
                    Stock = productDTO.Stock,
                    ProductCategories = new List<ProductCategory>(),
                    ProductImages = new List<ProductImage>()
                };

                foreach (var productImage in productDTO.ProductImages!)
                {
                    var photoProduct = Convert.FromBase64String(productImage);
                    newProduct.ProductImages.Add(new ProductImage { Image = await _fileStoragecs.SaveFileAsync(photoProduct, ".jpg", "products") });
                }

                foreach (var productCategoryId in productDTO.ProductCategoryIds!)
                {
                    var category = await _context.categories.FirstOrDefaultAsync(x => x.Id == productCategoryId);
                    if (category != null)
                    {
                        newProduct.ProductCategories.Add(new ProductCategory { Category = category });
                    }
                }

                _context.Add(newProduct);
                await _context.SaveChangesAsync();
                return new ActionResponse<Product>
                {
                    wasSuccess = true,
                    Result = newProduct
                };

            }
            catch ( DbUpdateException)
            {

                return new ActionResponse<Product>
                {
                    wasSuccess = false,
                    Message = "There is already a product with the same name."
                };

            }
            catch(Exception exception)
            {
                return new ActionResponse<Product>
                {
                    wasSuccess = false,
                    Message = exception.Message
                };

            }
        }

       

        public override async Task<ActionResponse<Product>> GetAsync(int id)
        {
            var product = await _context.Products
                .Include(x => x.ProductImages)
                .Include(x => x.ProductCategories!)
                .ThenInclude(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == id);
            if(product==null)
            {
                return new ActionResponse<Product>
                {
                    wasSuccess = false,
                    Message = "Product does not exist",
                };
            }
            return new ActionResponse<Product>
            {
                wasSuccess = true,
                Result = product,
            };
        }

        public override async Task<ActionResponse<IEnumerable<Product>>> GetAsync(PaginationDTO pagination)
        {
            var queryable =  _context.Products.Include(x => x.ProductCategories)
                .Include(x => x.ProductImages).AsQueryable();
            if (!string.IsNullOrEmpty(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }
            return new ActionResponse<IEnumerable<Product>>
            {
                wasSuccess = true,
                Result = queryable.OrderBy(x => x.Name)
                .Paginate(pagination).ToList()
            };
        }

        public override async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination)
        {
            var queryable = _context.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }
            double count = await queryable.CountAsync();
            int totalPages = (int)Math.Ceiling(count / pagination.RecordsNumber);
            return new ActionResponse<int>
            {
                wasSuccess = true,
                Result = totalPages
            };

        }

       

        public async Task<ActionResponse<Product>> UpdateFullAsync(ProductDTO productDTO)
        {
            try
            {
                var product = await _context.Products.Include(x => x.ProductCategories)
                .Include(x => x.ProductCategories).FirstOrDefaultAsync(x => x.Id == productDTO.Id);
                if (product == null)
                {
                    return new ActionResponse<Product>
                    {
                        wasSuccess = false,
                        Message = "Product does not exist",
                    };
                }
                product.Name = productDTO.Name;
                product.Description = productDTO.Description;
                product.Price = productDTO.Price;
                product.Stock = productDTO.Stock;
                _context.ProductCategories.RemoveRange(product.ProductCategories!);
                product.ProductCategories = new List<ProductCategory>();
                foreach (var productCategoryId in productDTO.ProductCategoryIds!)
                {
                    var category = await _context.categories.FindAsync(productCategoryId);
                    if (category != null)
                    {
                        _context.ProductCategories.Add(new ProductCategory { CategoryId = category.Id, ProductId = product.Id });

                    }
                }
                _context.Update(product);
                await _context.SaveChangesAsync();
                return new ActionResponse<Product>
                {
                    wasSuccess = true,
                    Result = product,
                };
            }
            catch (DbUpdateException)
            {
                return new ActionResponse<Product>
                {
                    wasSuccess = false,
                    Message = "There is already a product with the same name.",
                };
                
            }
            catch (Exception exception)
            {
                return new ActionResponse<Product>
                {
                    wasSuccess = false,
                    Message = exception.Message
                };

            }
        }
        public async Task<ActionResponse<ImageDTO>> AddImageAsync(ImageDTO imageDTO)
        {
            var product = await _context.Products.Include(x => x.ProductImages)
                .FirstOrDefaultAsync(x => x.Id == imageDTO.ProductId);
            if(product ==null)
            {
                return new ActionResponse<ImageDTO>
                {
                    wasSuccess = false,
                    Message = "product not exist"
                };
               
            }
            for (int i = 0; i < imageDTO.Images.Count; i++)
            {
                if (!imageDTO.Images[i].StartsWith("https://localhost:7051/images/products"))
                {
                    try
                    {
                        var photoProduct = Convert.FromBase64String(imageDTO.Images[i]);
                        imageDTO.Images[i] = await _fileStoragecs.SaveFileAsync(photoProduct, ".jpg", "products");
                        product.ProductImages!.Add(new ProductImage { Image = imageDTO.Images[i] });
                    }
                    catch (FormatException ex)
                    {

                        Console.WriteLine($"Error converting image at index {i}: {ex.Message}");
                    }
                }
            }
            _context.Update(product);
            await _context.SaveChangesAsync();
            return new ActionResponse<ImageDTO>
            {
                wasSuccess = true,
                Result = imageDTO,
            };
        }
        public async Task<ActionResponse<ImageDTO>> RemoveLastImageAsync(ImageDTO imageDTO)
        {
            var product = await _context.Products
                   .Include(x => x.ProductImages)
                   .FirstOrDefaultAsync(x => x.Id == imageDTO.ProductId);
            if (product == null)
            {
                return new ActionResponse<ImageDTO>
                {
                    wasSuccess = false,
                    Message = "Producto no existe"
                };
            }

            if (product.ProductImages is null || product.ProductImages.Count == 0)
            {
                return new ActionResponse<ImageDTO>
                {
                    wasSuccess = true,
                    Result = imageDTO
                };
            }

            var lastImage = product.ProductImages.LastOrDefault();
            await _fileStoragecs.RemoveFileAsync(lastImage!.Image, "products");
            _context.ProductImages.Remove(lastImage);

            await _context.SaveChangesAsync();
            imageDTO.Images = product.ProductImages.Select(x => x.Image).ToList();
            return new ActionResponse<ImageDTO>
            {
               wasSuccess = true,
                Result = imageDTO
            };

        }
    }
}
