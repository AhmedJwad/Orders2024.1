using Microsoft.AspNetCore.Components;
using Orders.frondEnd.Helpers;

namespace Orders.frondEnd.Shared
{
    public partial class MultipleSelector
    {
        private string addAllText = ">>";
        private string removeAllText = "<<";

        [Parameter]
        public List<MultipleSelectorModel> NonSelected { get; set; } = new();

        [Parameter]
        public List<MultipleSelectorModel> Selected { get; set; } = new();

        public void Select(MultipleSelectorModel model)
        {
            NonSelected.Remove(model);
            Selected.Add(model);
        }
        private void Unselect(MultipleSelectorModel item)
        {
            Selected.Remove(item);
            NonSelected.Add(item);
        }

        private void SelectAll()
        {
            Selected.AddRange(NonSelected);
            NonSelected.Clear();
        }

        private void UnselectAll()
        {
            NonSelected.AddRange(Selected);
            Selected.Clear();
        }


    }
}
