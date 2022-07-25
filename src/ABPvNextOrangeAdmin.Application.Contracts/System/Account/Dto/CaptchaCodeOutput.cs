namespace ABPvNextOrangeAdmin.System.Account.Dto;

public class CaptchaCodeOutput
{
    public string Uuid { get; set; }

    public byte[] ImgBytes { get; set; }

    private CaptchaCodeOutput(string uuid, byte[] imgBytes)
    {
        Uuid = uuid;
        ImgBytes = imgBytes;
    }

    public static CaptchaCodeOutput CreateInstance(string uuid, byte[] imgBytes)
    {
        return new CaptchaCodeOutput(uuid, imgBytes);
    }
}