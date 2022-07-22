using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ABPvNextOrangeAdmin.Config;
using ABPvNextOrangeAdmin.Utils.TextProducer;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Volo.Abp.DependencyInjection;

namespace ABPvNextOrangeAdmin.Utils.ImageProducer;

public class DefaultCaptcha : IImageProducer, ITransientDependency
{
    private int width = 200;
    private int height = 50;
    private CaptchaConfig _captchaConfig;

    public DefaultCaptcha(CaptchaConfig captchaConfig)
    {
        _captchaConfig = captchaConfig;
    }

    public byte[] createImage(String text)
    {
        // 新增图片
        Bitmap newBitmap = new Bitmap(text.Length * 20, 38);
        Graphics g = Graphics.FromImage(newBitmap);
        g.Clear(Color.White); // 图片清晰
        // 在图片上绘制文字
        SolidBrush solidBrush = new SolidBrush(Color.Red);
        g.DrawString(text, new Font("Aril", 18), solidBrush, 12, 4);
        // 绘制干扰线
        Random random = new Random(); // 随机
        for (int i = 0; i < 10; i++)
        {
            // 产生一条线，并绘制到画布的起始点（x，y）终点
            int x1 = random.Next(newBitmap.Width);
            int y1 = random.Next(newBitmap.Height);
            int x2 = random.Next(newBitmap.Width);
            int y2 = random.Next(newBitmap.Height);
            g.DrawLine(new Pen(Color.DarkGray), x1, y1, x2, y2);
        }

        // 绘制图片的干扰点
        for (int i = 0; i < 100; i++)
        {
            int x = random.Next(newBitmap.Width);
            int Y = random.Next(newBitmap.Height);
            newBitmap.SetPixel(x, Y, Color.FromArgb(random.Next()));
        }

        // 绘制边框
        g.DrawRectangle(new Pen(Color.Blue), 0, 0, newBitmap.Width, newBitmap.Height);
        g.DrawRectangle(new Pen(Color.Blue), -1, -1, newBitmap.Width, newBitmap.Height);
        // 将图片保存到内存流中
        MemoryStream ms = new MemoryStream();
        newBitmap.Save(ms, ImageFormat.Jpeg);
        return ms.ToArray(); // 将内存流写入byte数组返回
    }

    public String createText()
    {
        return ((ITextProducer)(new DefaultTextCreator().SetConfig(_captchaConfig))).getText();
    }
}