namespace CSLox;

internal class Return : Exception
{
    public object? value;

    public Return(object? value)
    {
        this.value = value;
    }
}
