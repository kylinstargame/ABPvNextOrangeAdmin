namespace ABPvNextOrangeAdmin.System.Account.Dto;

public class CaptchaCodeOutput
{
    public string Uuid { get; set; }

    public string Code { get; set; }

    private CaptchaCodeOutput(string uuid, string code)
    {
        Uuid = uuid;
        Code = code;
    }

    public static CaptchaCodeOutput CreateInstance(string uuid, string code)
    {
        return new CaptchaCodeOutput(uuid, code);
    }
}