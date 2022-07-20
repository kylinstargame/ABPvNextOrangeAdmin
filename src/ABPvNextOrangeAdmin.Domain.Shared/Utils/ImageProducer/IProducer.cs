using System;

namespace ABPvNextOrangeAdmin.Utils.ImageProducer;

public interface IProducer
{
    byte[] createImage(String var);

    String createText();
}