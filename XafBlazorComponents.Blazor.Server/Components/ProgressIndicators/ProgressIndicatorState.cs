namespace XafBlazorComponents.Blazor.Server.Components.ProgressIndicators
{
    public class ProgressIndicatorState
    {
        public int CurrentOperationPercentage { get; set; }
        public string OperationElementsDetail { get; set; }
        public string OperationCurrentElementDetail { get; set; }
        public bool Indeterminate { get; set; }
    }
}
