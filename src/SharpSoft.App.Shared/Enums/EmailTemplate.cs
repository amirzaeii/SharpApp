namespace SharpSoft.App.Shared.Enums;

[JsonConverter(typeof(JsonStringEnumConverter<EmailTemplate>))]
public enum EmailTemplate
{
    EmailChange,
    EmailConfirmation
}
