class UlidConverter :
    WriteOnlyJsonConverter<Ulid>
{
    public override void Write(VerifyJsonWriter writer, Ulid value)
    {
        if (!writer.Context.ScrubUlids())
        {
            writer.WriteValue(value.ToString());
            return;
        }

        var next = CounterContext.Current.Next(value);
        writer.WriteValue($"Ulid_{next}");
    }
}