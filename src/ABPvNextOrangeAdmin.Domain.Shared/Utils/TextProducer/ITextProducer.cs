using System;
using ABPvNextOrangeAdmin.Utils.ImageProducer;
using Microsoft.Extensions.Configuration;

namespace ABPvNextOrangeAdmin.Utils.TextProducer;

public interface ITextProducer : IProducer{
    String getText();

}