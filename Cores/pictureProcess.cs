using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;

namespace BeautyFace.Cores
{
    class pictureProcess
    {
        //获得高度和宽度
        public async Task<int[]> getWidthandHeight(StorageFile file)
        {
            Stream imagestream = await file.OpenStreamForReadAsync();
            BitmapDecoder dec = await BitmapDecoder.CreateAsync(imagestream.AsRandomAccessStream());
            int[] two=new int[2];
            two[0] = (int)dec.PixelWidth;
            two[1] = (int)dec.PixelHeight;
            return two;
        }
        //StorageFile转换成byte[]
        public async Task<byte[]> StorageFileToBytes(StorageFile file)
        {
            Stream imagestream = await file.OpenStreamForReadAsync();
            BitmapDecoder dec = await BitmapDecoder.CreateAsync(imagestream.AsRandomAccessStream());
            var data = await dec.GetPixelDataAsync();
            var bytes = data.DetachPixelData();
            return bytes;
        }
        //byte数组转换成能显示的图像
        public async Task<SoftwareBitmapSource> bytesArrayToshowImage(byte[] input,int width,int height)
        {
            var softwareBitmap = new SoftwareBitmap(BitmapPixelFormat.Bgra8, width,height, BitmapAlphaMode.Premultiplied);
            softwareBitmap.CopyFromBuffer(input.AsBuffer());
            var softwareBitmapSource = new SoftwareBitmapSource();
            await softwareBitmapSource.SetBitmapAsync(softwareBitmap);
            return softwareBitmapSource;
        }
        public byte[] blackEffect(byte[] old, int width, int height)
        {
            //黑白滤镜
            byte[] newOne = old;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < (int)width; j++)
                {
                    int k = (i * width + j) * 4;
                    if (old[k] > old[k + 1])
                    {
                        if (newOne[k] > newOne[k + 2])
                        {
                            newOne[k + 1] = newOne[k];
                            newOne[k + 2] = newOne[k];
                        }
                        else
                        {
                            newOne[k] = newOne[k + 2];
                            newOne[k + 1] = newOne[k + 2];
                        }
                    }
                    else
                    {
                        if (newOne[k + 1] > newOne[k + 2])
                        {
                            newOne[k] = newOne[k + 1];
                            newOne[k + 2] = newOne[k + 1];
                        }
                        else
                        {
                            newOne[k] = newOne[k + 2];
                            newOne[k + 1] = newOne[k + 2];
                        }
                    }
                }
            }
            return newOne;
        }
        public byte[] filmEffect(byte[] old, int width, int height)
        {
            //曝光滤镜
            byte[] newOne = old;
            byte[] shi = System.BitConverter.GetBytes(255);

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < (int)width; j++)
                {
                    int k = (i * width + j) * 4;
                    var newr = System.BitConverter.GetBytes(shi[0] - newOne[k]);
                    var newg = System.BitConverter.GetBytes(shi[0] - newOne[k + 1]);
                    var newb = System.BitConverter.GetBytes(shi[0] - newOne[k + 2]);
                    newOne[k] = newr[0];
                    newOne[k + 1] = newg[0];
                    newOne[k + 2] = newb[0];
                }
            }
            return newOne;
        }
        public byte[] engraveEffect(byte[] old, int width, int height)
        {
            //版画滤镜
            byte[] newOne = old;
            byte[] shi = System.BitConverter.GetBytes(128);
            for (int i = 0; i < height - 1; i++)
            {
                for (int j = 0; j < width - 1; j++)
                {
                    int k_1 = (i * width + j) * 4;
                    int k_2 = ((i + 1) * width + (j + 1)) * 4;
                    var newr = System.BitConverter.GetBytes(shi[0] - newOne[k_2] + newOne[k_1]);
                    var newg = System.BitConverter.GetBytes(shi[0] - newOne[k_2 + 1] + newOne[k_1 + 1]);
                    var newb = System.BitConverter.GetBytes(shi[0] - newOne[k_2 + 2] + newOne[k_1 + 2]);
                    newOne[k_1] = newr[0];
                    newOne[k_1 + 1] = newg[0];
                    newOne[k_1 + 2] = newb[0];
                }
            }
            return newOne;
        }
        public byte[] warmEffect(byte[] old, int width, int height)
        {
            //暖色滤镜
            byte[] newOne = old;
            for (int i = 0; i < height - 1; i++)
            {
                for (int j = 0; j < width - 1; j++)
                {
                    int k = (i * width + j) * 4;
                    int temp_1 = newOne[k + 1] + 50;
                    if (temp_1 > 255)
                    {
                        temp_1 = 255;
                    }
                    byte[] shi = System.BitConverter.GetBytes(temp_1);
                    newOne[k + 1] = shi[0];

                    int temp_2 = newOne[k + 2] + 50;
                    if (temp_2 > 255)
                    {
                        temp_2 = 255;
                    }
                    shi = System.BitConverter.GetBytes(temp_2);
                    newOne[k + 2] = shi[0];
                }
            }
            return newOne;
        }
        public byte[] coldEffect(byte[] old, int width, int height)
        {
            //冷色滤镜
            byte[] newOne = old;
            int temp;
            for (int i = 0; i < height - 1; i++)
            {
                for (int j = 0; j < width - 1; j++)
                {
                    int k = (i * width + j) * 4;
                    temp = newOne[k] + 70;
                    if (temp > 255)
                    {
                        temp = 255;
                    }
                    byte[] shi = System.BitConverter.GetBytes(temp);
                    newOne[k] = shi[0];
                }
            }
            return newOne;
        }
        public Color GetPixel(byte[] pixels, int x, int y, uint width, uint height)
        {
            //获取某像素点的Color值
            //http://blog.csdn.net/jiangxinyu/article/details/6222322
            byte[] shi = System.BitConverter.GetBytes(255);
            int i = x;
            int j = y;
            int k = (i * (int)width + j) * 3;
            var r = pixels[k + 0];
            var newr = System.BitConverter.GetBytes(shi[0] - r);
            var g = pixels[k + 1];
            var b = pixels[k + 2];
            return Color.FromArgb(0, newr[0], g, b);
        }
        public byte[] lightenRGBPixel(byte[] rgb, double scale)
        {
            //亮度增强
            //RGB转换YUV
            //Y'= 0.299*R' + 0.587 * G' + 0.114*B'
            //U'= -0.147*R' - 0.289 * G' + 0.436*B' = 0.492 * (B'- Y')
            //V'= 0.615*R' - 0.515 * G' - 0.100*B' = 0.877 * (R'- Y')

            //YUV转换成RGB
            //R' = Y' + 1.140 * V'
            //G' = Y' - 0.394 * U' - 0.581*V'
            //B' = Y' + 2.032 * U'
            double b = rgb[0];
            double g = rgb[1];
            double r = rgb[2];
            double y = 0.299 * r + 0.587 * g + 0.114 * b + scale;
            double u = -0.147 * r - 0.289 * g + 0.436 * b;
            double v = 0.615 * r - 0.515 * g - 0.100 * b;
            int b_ = (int)System.Math.Round(y + 2.032 * u);
            int g_ = (int)System.Math.Round(y - 0.394 * u - 0.581 * v);
            int r_ = (int)System.Math.Round(y + 1.140 * v);
            if (b_ > 255) b_ = 255;
            if (b_ < 0) b_ = 0;
            if (g_ > 255) g_ = 255;
            if (g_ < 0) g_ = 0;
            if (r_ > 255) r_ = 255;
            if (r_ < 0) r_ = 0;
            byte[] newRGB = new byte[3];
            byte[] temp1 = System.BitConverter.GetBytes(b_);
            newRGB[0] = temp1[0];
            byte[] temp2 = System.BitConverter.GetBytes(g_);
            newRGB[1] = temp2[0];
            byte[] temp3 = System.BitConverter.GetBytes(r_);
            newRGB[2] = temp3[0];
            return newRGB;
        }
        public byte[] increBright(byte[] old, int height, int width, double scale)
        {
            // 亮度增强，scale取值范围可以是【-255,255】的double
            byte[] newOne = old;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    int k = (i * width + j) * 4;
                    byte[] inputBytes = new byte[3];
                    inputBytes[0] = newOne[k];   //b通道
                    inputBytes[1] = newOne[k + 1]; //g通道
                    inputBytes[2] = newOne[k + 2]; //r通道
                    byte[] outputBytes = lightenRGBPixel(inputBytes, scale);
                    newOne[k] = outputBytes[0];
                    newOne[k + 1] = outputBytes[1];
                    newOne[k + 2] = outputBytes[2];
                }
            }
            return newOne;
        }
        public byte[] gaussBlur(byte[] old, int height, int width, int kernelSize)
        {
            //高斯模糊
            //高斯核的产生 G(x,y)=1/(2*pi*sigma)*e^(-(x^2+y^2)/(2*sigma^2))
            double sigma = 1.5;
            double[,] kernel = new double[kernelSize, kernelSize];    //高斯核
            for (int i = 0; i < kernelSize; i++)
            {
                for (int j = 0; j < kernelSize; j++)
                {
                    int k = (i * width + j) * 4;
                    double x = i - (kernelSize - 1) / 2;
                    double y = j - (kernelSize - 1) / 2;
                    kernel[i, j] = 1 / (2 * System.Math.PI * System.Math.Pow(sigma, 2)) * System.Math.Exp(-(System.Math.Pow(x, 2) + System.Math.Pow(y, 2)) / (2 * System.Math.Pow(sigma, 2)));
                }
            }
            byte[] newOne = old;
            Debug.WriteLine(height);
            Debug.WriteLine(width);
            for (int i = (kernelSize - 1) / 2; i < height - (kernelSize - 1) / 2; i++)
            {
                for (int j = (kernelSize - 1) / 2; j < width - (kernelSize - 1) / 2; j++)
                {
                    int k = (i * width + j) * 4;
                    byte[] newColor = modelOperation(i, j, width, old, kernel, kernelSize);
                    newOne[k] = newColor[0];
                    newOne[k + 1] = newColor[1];
                    newOne[k + 2] = newColor[2];
                }
            }
            return newOne;
        }
        public byte[] modelOperation(int x, int y, int width, byte[] old, double[,] model, int kernelSize)
        {
            //对x，y的像素执行模板操作
            double[] result = new double[3];
            result[0] = 0;
            result[1] = 0;
            result[2] = 0;
            for (int i = 0; i < kernelSize; i++)
            {
                for (int j = 0; j < kernelSize; j++)
                {
                    int rep_x = i - (kernelSize - 1) / 2;
                    int rep_y = j - (kernelSize - 1) / 2;
                    int k = ((x + rep_x) * width + (y + rep_y)) * 4;
                    result[0] = result[0] + model[i, j] * old[k];
                    result[1] = result[1] + model[i, j] * old[k + 1];
                    result[2] = result[2] + model[i, j] * old[k + 2];
                }
            }
            byte[] outColor = new byte[3];
            byte[] temp1 = System.BitConverter.GetBytes((int)System.Math.Round(result[0]));
            outColor[0] = temp1[0];
            byte[] temp2 = System.BitConverter.GetBytes((int)System.Math.Round(result[1]));
            outColor[1] = temp2[0];
            byte[] temp3 = System.BitConverter.GetBytes((int)System.Math.Round(result[2]));
            outColor[2] = temp3[0];
            return outColor;
        }
        public byte[] colorLips(byte[] old, int width, int height, byte[] color, int mouthLeft_x, int mouthLeft_y, int mouthRight_x, int mouthRight_y,int mouthUp_x,int mouthUp_y,int mouthDown_x,int mouthDown_y)
        {
            double delta1= (mouthLeft_y - mouthUp_y)/ (double)((mouthRight_x - mouthLeft_x)/2);
            double delta2= (mouthLeft_y - mouthDown_y) / (double)((mouthRight_x - mouthLeft_x) / 2);
            byte[] newOne = old;

            //参照位置
            int refLoc= (mouthLeft_x * width + mouthLeft_x) * 4;
            //上唇
            for (int j = mouthLeft_x; j < mouthLeft_x + (mouthRight_x - mouthLeft_x) / 2; j++)
            {
                for (int i = (int)(mouthLeft_y - delta1 * (j - mouthLeft_x)); i <= mouthLeft_y; i++)
                {
                    int k = (i * width + j) * 4;
                   if(System.Math.Abs(old[refLoc]-newOne[k])<5&&System.Math.Abs(old[refLoc+1]-newOne[k+1])<5&&System.Math.Abs(old[refLoc+2]-newOne[k+2])<5)
                    {
                        newOne[k] = color[0];
                        newOne[k + 1] = color[1];
                        newOne[k + 2] = color[2];
                    }                
                }
            }
            for (int j = mouthLeft_x + (mouthRight_x - mouthLeft_x) / 2; j <= mouthRight_x; j++)
            {
                for (int i = (int)(mouthLeft_y-delta1 * (mouthRight_x-j)); i <= mouthLeft_y; i++)
                {
                    int k = (i * width + j) * 4;
                    newOne[k] = color[0];
                    newOne[k + 1] = color[1];
                    newOne[k + 2] = color[2];
                }
            }

            //下唇
            for (int j = mouthLeft_x; j < mouthLeft_x + (mouthRight_x - mouthLeft_x) / 2; j++)
            {
                for (int i = mouthLeft_y; i <= (int)(mouthLeft_y + delta1 * (j - mouthLeft_x)); i++)
                {
                    int k = (i * width + j) * 4;
                    newOne[k] = color[0];
                    newOne[k + 1] = color[1];
                    newOne[k + 2] = color[2];
                }
            }
            for (int j = mouthLeft_x + (mouthRight_x - mouthLeft_x) / 2; j <= mouthRight_x; j++)
            {
                for (int i = mouthLeft_y; i <= (int)(mouthLeft_y + delta1 * (mouthRight_x - j)); i++)
                {
                    int k = (i * width + j) * 4;
                    newOne[k] = color[0];
                    newOne[k + 1] = color[1];
                    newOne[k + 2] = color[2];
                }
            }
            return newOne;
        }
        public byte[] brightFace(byte[] old, int width, int height,int face_left,int face_top,int face_right,int face_low)
        {
            byte[] newOne = old;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    int k = (i * width + j) * 4;
                    if (i>=face_top && i <= face_low && j>=face_left && j<=face_right)
                    {
                        byte[] inputBytes = new byte[3];
                        inputBytes[0] = newOne[k];   //b通道
                        inputBytes[1] = newOne[k + 1]; //g通道
                        inputBytes[2] = newOne[k + 2]; //r通道
                        byte[] outputBytes = lightenRGBPixel(inputBytes, 18);
                        newOne[k] = outputBytes[0];
                        newOne[k + 1] = outputBytes[1];
                        newOne[k + 2] = outputBytes[2];
                    }
                    else
                    {
                        byte[] inputBytes = new byte[3];
                        inputBytes[0] = newOne[k];   //b通道
                        inputBytes[1] = newOne[k + 1]; //g通道
                        inputBytes[2] = newOne[k + 2]; //r通道
                        byte[] outputBytes = lightenRGBPixel(inputBytes, 13);
                        newOne[k] = outputBytes[0];
                        newOne[k + 1] = outputBytes[1];
                        newOne[k + 2] = outputBytes[2];
                    }
                    
                }
            }
           // byte[] newOne2 = meanFilter(newOne, height, width);
            return newOne;
        }
        public byte[] brightEyes(byte[] old,int width,int height,int leftEye_left,int leftEye_top,int leftEye_right,int leftEye_low,
            int rightEye_left,int rightEye_top,int rightEye_right,int rightEye_low)
        {
            byte[] newOne = old;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (i >= leftEye_top && i <= leftEye_low && j >=leftEye_left && j <= leftEye_right)
                    {
                        int k = (i * width + j) * 4;
                        byte[] inputBytes = new byte[3];
                        inputBytes[0] = newOne[k];   //b通道
                        inputBytes[1] = newOne[k + 1]; //g通道
                        inputBytes[2] = newOne[k + 2]; //r通道
                        byte[] outputBytes = lightenRGBPixel(inputBytes, 40);
                        newOne[k] = outputBytes[0];
                        newOne[k + 1] = outputBytes[1];
                        newOne[k + 2] = outputBytes[2];
                    }
                    else if (i >= rightEye_top && i <= rightEye_low && j >= rightEye_left && j <= rightEye_right)
                    {
                        int k = (i * width + j) * 4;
                        byte[] inputBytes = new byte[3];
                        inputBytes[0] = newOne[k];   //b通道
                        inputBytes[1] = newOne[k + 1]; //g通道
                        inputBytes[2] = newOne[k + 2]; //r通道
                        byte[] outputBytes = lightenRGBPixel(inputBytes, 40);
                        newOne[k] = outputBytes[0];
                        newOne[k + 1] = outputBytes[1];
                        newOne[k + 2] = outputBytes[2];
                    }
                    else
                    {
                        int k = (i * width + j) * 4;
                        byte[] inputBytes = new byte[3];
                        inputBytes[0] = newOne[k];   //b通道
                        inputBytes[1] = newOne[k + 1]; //g通道
                        inputBytes[2] = newOne[k + 2]; //r通道
                        byte[] outputBytes = lightenRGBPixel(inputBytes, 20);
                        newOne[k] = outputBytes[0];
                        newOne[k + 1] = outputBytes[1];
                        newOne[k + 2] = outputBytes[2];
                    }
                }
            }
            return newOne;
        }
        public byte[] meanFilter(byte[] old, int height, int width)
        {
            double[,] kernel = new double[3, 3];    //高斯核
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    int k = (i * width + j) * 4;
                    double x = i - (3 - 1) / 2;
                    double y = j - (3 - 1) / 2;
                    kernel[i, j] = (double)1 / 9;
                }
            }
            byte[] newOne = old;
            for (int i = 1; i < height - 1; i++)
            {
                for (int j = 1; j < width - 1; j++)
                {
                    int k = (i * width + j) * 4;
                    byte[] newColor = modelOperation(i, j, width, old, kernel, 3);
                    newOne[k] = newColor[0];
                    newOne[k + 1] = newColor[1];
                    newOne[k + 2] = newColor[2];
                }
            }
            return newOne;
        }
        public byte[] matrixMinusNum(byte[] old,int num)
        {
            byte[] shi = System.BitConverter.GetBytes(num);
            for (int i=0;i<old.Length;i++)
            {
                byte[] res = System.BitConverter.GetBytes(old[i] - shi[0]);
                old[i] = res[0];
            }
            return old;
        }
    }
}
