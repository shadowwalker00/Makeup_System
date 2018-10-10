using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace BeautyFace
{
    class FaceProxy
    {
        public async static Task<byte[]> GetImageAsByteArray(StorageFile file)
        {
            byte[] fileBytes = null;
            using (IRandomAccessStreamWithContentType stream = await file.OpenReadAsync())
            {
                fileBytes = new byte[stream.Size];
                using (DataReader reader = new DataReader(stream))
                {
                    await reader.LoadAsync((uint)stream.Size);
                    reader.ReadBytes(fileBytes);
                }
            }
            return fileBytes;
        }
        public async static Task<RootObject> GetFace(StorageFile sampleFile)
        {

            var client = new HttpClient();
            // Request headers - replace this example key with your valid key.
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "d23b815706794b60b9a8756982f64042 ");
            // Request parameters and URI string.
            //string queryString = "returnFaceId=true&returnFaceLandmarks=true&returnFaceAttributes=age,gender,emotion";
            string uri = "https://westus.api.cognitive.microsoft.com/face/v1.0/detect?returnFaceId=true&returnFaceLandmarks=true&returnFaceAttributes=age,gender,emotion";
            HttpResponseMessage response;
            string responseContent;
            // Request body. Try this sample with a locally stored JPEG image.
            byte[] byteData = await GetImageAsByteArray(sampleFile);
            RootObject data;
            using (var content = new ByteArrayContent(byteData))
            {
                // This example uses content type "application/octet-stream".
                // The other content types you can use are "application/json" and "multipart/form-data".
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                try
                {
                    response = await client.PostAsync(uri, content);
                    responseContent = response.Content.ReadAsStringAsync().Result;
                    var serializer = new DataContractJsonSerializer(typeof(RootObject));
                    responseContent = responseContent.Substring(1, responseContent.Length - 2);
                    byte[] byteArray = Encoding.UTF8.GetBytes(responseContent);
                    MemoryStream ms = new MemoryStream(byteArray);
                    data = (RootObject)serializer.ReadObject(ms);
                    return data;
                }
                catch (HttpRequestException e)
                {
                    Debug.WriteLine(e.InnerException.Message);
                    return null;
                }
            }
        }
    }

    [DataContract]
    public class FaceRectangle
    {
        [DataMember]
        public int top { get; set; }
        [DataMember]
        public int left { get; set; }
        [DataMember]
        public int width { get; set; }
        [DataMember]
        public int height { get; set; }
    }

    [DataContract]
    public class PupilLeft
    {
        [DataMember]
        public double x { get; set; }
        [DataMember]
        public double y { get; set; }
    }

    [DataContract]
    public class PupilRight
    {
        [DataMember]
        public double x { get; set; }

        [DataMember]
        public double y { get; set; }
    }

    [DataContract]
    public class NoseTip
    {
        [DataMember]
        public double x { get; set; }

        [DataMember]
        public double y { get; set; }
    }

    [DataContract]
    public class MouthLeft
    {
        [DataMember]
        public double x { get; set; }

        [DataMember]
        public double y { get; set; }
    }

    [DataContract]
    public class MouthRight
    {
        [DataMember]
        public double x { get; set; }

        [DataMember]
        public double y { get; set; }
    }

    [DataContract]
    public class EyebrowLeftOuter
    {
        [DataMember]
        public double x { get; set; }

        [DataMember]
        public double y { get; set; }
    }

    [DataContract]
    public class EyebrowLeftInner
    {
        [DataMember]
        public double x { get; set; }

        [DataMember]
        public double y { get; set; }
    }

    [DataContract]
    public class EyeLeftOuter
    {
        [DataMember]
        public double x { get; set; }

        [DataMember]
        public double y { get; set; }
    }

    [DataContract]
    public class EyeLeftTop
    {
        [DataMember]
        public double x { get; set; }

        [DataMember]
        public double y { get; set; }
    }

    [DataContract]
    public class EyeLeftBottom
    {
        [DataMember]
        public double x { get; set; }

        [DataMember]
        public double y { get; set; }
    }

    [DataContract]
    public class EyeLeftInner
    {
        [DataMember]
        public double x { get; set; }

        [DataMember]
        public double y { get; set; }
    }

    [DataContract]
    public class EyebrowRightInner
    {
        [DataMember]
        public double x { get; set; }

        [DataMember]
        public double y { get; set; }
    }

    [DataContract]
    public class EyebrowRightOuter
    {
        [DataMember]
        public double x { get; set; }
        [DataMember]
        public double y { get; set; }
    }

    [DataContract]
    public class EyeRightInner
    {
        [DataMember]
        public double x { get; set; }

        [DataMember]
        public double y { get; set; }
    }

    [DataContract]
    public class EyeRightTop
    {
        [DataMember]
        public double x { get; set; }

        [DataMember]
        public double y { get; set; }
    }

    [DataContract]
    public class EyeRightBottom
    {
        [DataMember]
        public double x { get; set; }

        [DataMember]
        public double y { get; set; }
    }

    [DataContract]
    public class EyeRightOuter
    {
        [DataMember]
        public double x { get; set; }

        [DataMember]
        public double y { get; set; }
    }

    [DataContract]
    public class NoseRootLeft
    {
        [DataMember]
        public double x { get; set; }

        [DataMember]
        public double y { get; set; }
    }

    [DataContract]
    public class NoseRootRight
    {
        [DataMember]
        public double x { get; set; }

        [DataMember]
        public double y { get; set; }
    }

    [DataContract]
    public class NoseLeftAlarTop
    {
        [DataMember]
        public double x { get; set; }
        [DataMember]
        public double y { get; set; }
    }

    [DataContract]
    public class NoseRightAlarTop
    {
        [DataMember]
        public double x { get; set; }

        [DataMember]
        public double y { get; set; }
    }

    [DataContract]
    public class NoseLeftAlarOutTip
    {
        [DataMember]
        public double x { get; set; }

        [DataMember]
        public double y { get; set; }
    }

    [DataContract]
    public class NoseRightAlarOutTip
    {
        [DataMember]
        public double x { get; set; }
        [DataMember]
        public double y { get; set; }
    }

    [DataContract]
    public class UpperLipTop
    {
        [DataMember]
        public double x { get; set; }
        [DataMember]
        public double y { get; set; }
    }

    [DataContract]
    public class UpperLipBottom
    {
        [DataMember]
        public double x { get; set; }
        [DataMember]
        public double y { get; set; }
    }

    [DataContract]
    public class UnderLipTop
    {
        [DataMember]
        public double x { get; set; }
        [DataMember]
        public double y { get; set; }
    }

    [DataContract]
    public class UnderLipBottom
    {
        [DataMember]
        public double x { get; set; }
        [DataMember]
        public double y { get; set; }
    }

    [DataContract]
    public class FaceLandmarks
    {
        [DataMember]
        public PupilLeft pupilLeft { get; set; }

        [DataMember]
        public PupilRight pupilRight { get; set; }

        [DataMember]
        public NoseTip noseTip { get; set; }

        [DataMember]
        public MouthLeft mouthLeft { get; set; }

        [DataMember]
        public MouthRight mouthRight { get; set; }

        [DataMember]
        public EyebrowLeftOuter eyebrowLeftOuter { get; set; }

        [DataMember]
        public EyebrowLeftInner eyebrowLeftInner { get; set; }

        [DataMember]
        public EyeLeftOuter eyeLeftOuter { get; set; }

        [DataMember]
        public EyeLeftTop eyeLeftTop { get; set; }

        [DataMember]
        public EyeLeftBottom eyeLeftBottom { get; set; }

        [DataMember]
        public EyeLeftInner eyeLeftInner { get; set; }

        [DataMember]
        public EyebrowRightInner eyebrowRightInner { get; set; }

        [DataMember]
        public EyebrowRightOuter eyebrowRightOuter { get; set; }

        [DataMember]
        public EyeRightInner eyeRightInner { get; set; }

        [DataMember]
        public EyeRightTop eyeRightTop { get; set; }

        [DataMember]
        public EyeRightBottom eyeRightBottom { get; set; }

        [DataMember]
        public EyeRightOuter eyeRightOuter { get; set; }

        [DataMember]
        public NoseRootLeft noseRootLeft { get; set; }

        [DataMember]
        public NoseRootRight noseRootRight { get; set; }
        [DataMember]
        public NoseLeftAlarTop noseLeftAlarTop { get; set; }

        [DataMember]
        public NoseRightAlarTop noseRightAlarTop { get; set; }

        [DataMember]
        public NoseLeftAlarOutTip noseLeftAlarOutTip { get; set; }

        [DataMember]
        public NoseRightAlarOutTip noseRightAlarOutTip { get; set; }

        [DataMember]
        public UpperLipTop upperLipTop { get; set; }

        [DataMember]
        public UpperLipBottom upperLipBottom { get; set; }

        [DataMember]
        public UnderLipTop underLipTop { get; set; }

        [DataMember]
        public UnderLipBottom underLipBottom { get; set; }
    }

    [DataContract]
    public class Emotion
    {

        [DataMember]
        public double anger { get; set; }

        [DataMember]
        public double contempt { get; set; }

        [DataMember]
        public double disgust { get; set; }

        [DataMember]
        public double fear { get; set; }

        [DataMember]
        public double happiness { get; set; }

        [DataMember]
        public double neutral { get; set; }

        [DataMember]
        public double sadness { get; set; }

        [DataMember]
        public double surprise { get; set; }
    }

    [DataContract]
    public class FaceAttributes
    {
        [DataMember]
        public string gender { get; set; }

        [DataMember]
        public double age { get; set; }

        [DataMember]
        public Emotion emotion { get; set; }
    }

    [DataContract]
    public class RootObject
    {
        [DataMember]
        public string faceId { get; set; }

        [DataMember]
        public FaceRectangle faceRectangle { get; set; }

        [DataMember]
        public FaceLandmarks faceLandmarks { get; set; }

        [DataMember]
        public FaceAttributes faceAttributes { get; set; }
    }
}
