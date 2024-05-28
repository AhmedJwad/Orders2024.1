using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Orders.frondEnd.Repositories;
using Orders.Shared.DTOs;
using Orders.Shared.Entities;

namespace Orders.frondEnd.Pages.Products
{
    [Authorize (Roles ="Admin")]
    public partial class ProductEdit
    {
        private ProductDTO ProductDTO = new()
        {
            ProductCategoryIds= new List<int>(),
            ProductImages= new List<string>(),
        };
        private ProductForm? productForm;
        private List<Category> selectedCategories = new();
        private List<Category> nonSelectedCategories = new();
        private bool loading = true;
        private Product? product;
        [Parameter] public int  productId { get; set; }
        [Inject] private IRepository repository { get; set; } = null!;
        [Inject] private NavigationManager navigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            await LoadProductAsync();
            await LoadCategoriesAsync();

        }
        private async Task AddImageAsync()
        {
            if (ProductDTO.ProductImages is not null &&  ProductDTO.ProductImages.Count==0)
            {
                return;
            }
            var imageDTO = new ImageDTO
            {
                ProductId= productId,
                Images=ProductDTO.ProductImages!,
            };
            var httpActionRespse = await repository.PostAsync<ImageDTO, ImageDTO>("/api/products/addImages", imageDTO);
            if (httpActionRespse.Error)
            {
                var message = await httpActionRespse.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            ProductDTO.ProductImages = httpActionRespse.Response!.Images;
            var toast = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000
            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Images added successfully.");


        }

        private async Task RemoveImageAsyc()
        {
            if (ProductDTO.ProductImages is null || ProductDTO.ProductImages.Count == 0)
            {
                return;
            }

            var imageDTO = new ImageDTO
            {
                ProductId = productId,
                Images = ProductDTO.ProductImages!
            };

            var httpActionResponse = await repository.PostAsync<ImageDTO, ImageDTO>("/api/products/removeLastImage", imageDTO);
            if (httpActionResponse.Error)
            {
                var message = await httpActionResponse.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            ProductDTO.ProductImages = httpActionResponse.Response!.Images;
            var toast = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000
            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Image successfully deleted.");

        }

        private async Task LoadCategoriesAsync()
        {
            loading = true;
            var httpActionResponse = await repository.GetASync<List<Category>>("/api/Categories/combo");
            if (httpActionResponse.Error)
            {
                loading = false;
                var message = await httpActionResponse.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            var categories = httpActionResponse.Response!;
            foreach (var category in categories)
            {
                var found = product!.ProductCategories!.FirstOrDefault(x => x.CategoryId == category.Id);
                if (found is null)
                {
                    nonSelectedCategories.Add(category);
                }
                else
                {
                    selectedCategories.Add(category);
                }
            }
            loading = false;

        }

        private async Task LoadProductAsync()
        {
            loading = true;
            var httpActionResponse = await repository.GetASync<Product>($"/api/products/{productId}");
            if (httpActionResponse.Error)
            {
                loading = false;
                var message = await httpActionResponse.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            product = httpActionResponse.Response!;
            ProductDTO = ToProductDTO(product);
            loading = false;

        }

        private ProductDTO ToProductDTO(Product product)
        {
            return new ProductDTO
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                Id = product.Id,
                ProductCategoryIds = product.ProductCategories!.Select(x => x.CategoryId).ToList(),
                ProductImages = product.ProductImages!.Select(x => x.Image).ToList(),

            };
        }
        private async Task SaveChangesAsync()
        {
            var httpActionResponse = await repository.PutAsync("/api/products/full", ProductDTO);
            if (httpActionResponse.Error)
            {
                var message = await httpActionResponse.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            Return();

        }

        private void Return()
        {
            productForm!.FormPostedSuccessfully = true;
            navigationManager.NavigateTo($"/products");

        }
    }
}
