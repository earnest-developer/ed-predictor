namespace Predictor.Logging
{
    public interface IDiagnosticContextAdaptor
    {
        //
        // Summary:
        //     Set the specified property on the current diagnostic context. The property will
        //     be collected and attached to the event emitted at the completion of the context.
        //
        // Parameters:
        //   propertyName:
        //     The name of the property. Must be non-empty.
        //
        //   value:
        //     The property value.
        //
        //   destructureObjects:
        //     If true, the value will be serialized as structured data if possible; if false,
        //     the object will be recorded as a scalar or simple array.
        void Set(string propertyName, object value, bool destructureObjects = false);
    }
}