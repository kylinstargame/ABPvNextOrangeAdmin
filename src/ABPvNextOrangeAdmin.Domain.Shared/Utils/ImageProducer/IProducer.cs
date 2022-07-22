using System;
using ABPvNextOrangeAdmin.Config;

namespace ABPvNextOrangeAdmin.Utils.ImageProducer;

public interface IProducer
{
    
}
public interface IImageProducer : IProducer
{
    byte[] createImage(String var);

    String createText();
}


public interface IWithConfig
{
    public IProducer SetConfig(CaptchaConfig config);
}