using Microsoft.AspNetCore.Components;

namespace Orders.frondEnd.Shared
{
    public partial class GenericList<Titem>
    {
        [Parameter]public RenderFragment? Loading { get; set; }
        [Parameter] public RenderFragment? Norecord { get; set; }
        [EditorRequired , Parameter]public RenderFragment body { get; set; }=null;
        [EditorRequired, Parameter] public List<Titem> MyList { get; set; } = null;

    }
}
