using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MudBlazor;

namespace Orders.frondEnd.Shared
{
    public partial class CarouselView
    {
        private bool arrows = true;
        private bool bullets = true;
        private bool enableSwipeGesture = true;
        private bool autocycle=true;
        private Transition transition= new Transition();
        [Parameter, EditorRequired] public List<string> Images { get; set; } = null!;

    }
}
