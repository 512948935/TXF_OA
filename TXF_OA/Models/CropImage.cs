using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace TXF_OA.Models
{
    public class CropImage
    {
        /// <summary>
        /// 剪裁头像图片
        /// </summary>
        /// <param name="pointX">X坐标</param>
        /// <param name="pointY">Y坐标</param>
        /// <param name="imgUrl">被截图图片地址</param>
        /// <param name="rlSize">截图矩形的大小</param>
        public static string CutImage(string imgUrl, int pointX = 0, int pointY = 0, int width = 0, int height = 0, int finalWidth = 180, int finalHeight = 180, string suffix = "")
        {
            Bitmap bitmap = null;   //按截图区域生成Bitmap
            Image thumbImg = null;  //被截图 
            Graphics gps = null;    //存绘图对象   
            Image finalImg = null;  //最终图片
            try
            {
                if (!string.IsNullOrEmpty(imgUrl))
                {
                    bitmap = new Bitmap(width, height);
                    thumbImg = Image.FromFile(HttpContext.Current.Server.MapPath(imgUrl));
                    gps = Graphics.FromImage(bitmap);      //读到绘图对象
                    gps.DrawImage(thumbImg, new Rectangle(0, 0, width, height), new Rectangle(pointX, pointY, width, height), GraphicsUnit.Pixel);
                    finalImg = GetThumbNailImage(bitmap, finalWidth, finalHeight, false);
                    //以下代码为保存图片时，设置压缩质量  
                    EncoderParameters ep = new EncoderParameters();
                    long[] qy = new long[1];
                    qy[0] = 80;//设置压缩的比例1-100  
                    EncoderParameter eParam = new EncoderParameter(Encoder.Quality, qy);
                    ep.Param[0] = eParam;

                    ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                    ImageCodecInfo jpegICIinfo = null;
                    for (int x = 0; x < arrayICI.Length; x++)
                    {
                        if (arrayICI[x].FormatDescription.Equals("JPEG"))
                        {
                            jpegICIinfo = arrayICI[x];
                            break;
                        }
                    }
                    string finalUrl = imgUrl.Replace("TempImg", "HeaderImg");
                    Regex reg = new Regex(".");
                    finalUrl = reg.Replace(finalUrl, suffix + ".", 1, finalUrl.LastIndexOf("."));
                    string finalPath = HttpContext.Current.Server.MapPath(finalUrl);
                    string finalPathDir = finalPath.Substring(0, finalPath.LastIndexOf("\\"));
                    if (!Directory.Exists(finalPathDir))
                    {
                        Directory.CreateDirectory(finalPathDir);
                    }
                    if (jpegICIinfo != null)
                    {
                        finalImg.Save(finalPath, jpegICIinfo, ep);
                    }
                    else
                    {
                        finalImg.Save(finalPath);
                    }
                    return finalUrl;
                }
                return "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bitmap.Dispose();
                thumbImg.Dispose();
                gps.Dispose();
                finalImg.Dispose();
                GC.Collect();
            }
        }

        ///<summary>
        /// 对给定的一个图片（Image对象）生成一个指定大小的缩略图。
        ///</summary>
        ///<param name="originalImage">原始图片</param>
        ///<param name="thumMaxWidth">缩略图的宽度</param>
        ///<param name="thumMaxHeight">缩略图的高度</param>
        ///<param name="isRatio">是否按照比例缩放</param>
        ///<returns>返回缩略图的Image对象</returns>
        public static Image GetThumbNailImage(Image originalImage, int thumMaxWidth, int thumMaxHeight, bool isRatio)
        {
            Size thumRealSize = Size.Empty;
            Image newImage = originalImage;
            Graphics graphics = null;
            try
            {
                if (isRatio)
                    thumRealSize = GetNewSize(thumMaxWidth, thumMaxHeight, originalImage.Width, originalImage.Height);
                else
                    thumRealSize = new Size(thumMaxWidth, thumMaxHeight);
                newImage = new Bitmap(thumRealSize.Width, thumRealSize.Height);
                graphics = Graphics.FromImage(newImage);
                graphics.DrawImage(originalImage, new Rectangle(0, 0, thumRealSize.Width, thumRealSize.Height), new Rectangle(0, 0, originalImage.Width, originalImage.Height), GraphicsUnit.Pixel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (graphics != null)
                {
                    graphics.Dispose();
                    graphics = null;
                }
            }
            return newImage;
        }
        ///<summary>
        /// 获取一个图片按等比例缩小后的大小。
        ///</summary>
        ///<param name="maxWidth">需要缩小到的宽度</param>
        ///<param name="maxHeight">需要缩小到的高度</param>
        ///<param name="imageOriginalWidth">图片的原始宽度</param>
        ///<param name="imageOriginalHeight">图片的原始高度</param>
        ///<returns>返回图片按等比例缩小后的实际大小</returns>
        public static Size GetNewSize(int maxWidth, int maxHeight, int imageOriginalWidth, int imageOriginalHeight)
        {
            double w = 0.0;
            double h = 0.0;
            double sw = Convert.ToDouble(imageOriginalWidth);
            double sh = Convert.ToDouble(imageOriginalHeight);
            double mw = Convert.ToDouble(maxWidth);
            double mh = Convert.ToDouble(maxHeight);
            if (sw < mw && sh < mh)
            {
                w = sw;
                h = sh;
            }
            else if ((sw / sh) > (mw / mh))
            {
                w = maxWidth;
                h = (w * sh) / sw;
            }
            else
            {
                h = maxHeight;
                w = (h * sw) / sh;
            }
            return new Size(Convert.ToInt32(w), Convert.ToInt32(h));
        }
    }
}