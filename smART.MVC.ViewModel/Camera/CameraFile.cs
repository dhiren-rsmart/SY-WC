using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;

namespace smART.ViewModel {
  public class CameraFile : HttpPostedFileBase {
    Stream stream;
    string contentType;
    string fileName;

    public CameraFile(Stream stream, string contentType, string fileName) {
      this.stream = stream;
      this.contentType = contentType;
      this.fileName = fileName;
    }

    public override int ContentLength {
      get { return (int)stream.Length; }
    }

    public override string ContentType {
      get { return contentType; }
    }

    public override string FileName {
      get { return fileName; }
    }

    public override Stream InputStream {
      get { return stream; }
    }

    public override void SaveAs(string filename) {
      using (var file = File.Open(filename, FileMode.CreateNew))
        stream.CopyTo(file);
    }
  }
}
