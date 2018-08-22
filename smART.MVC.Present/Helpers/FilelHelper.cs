using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using smART.MVC.Present.Helpers;
using ICSharpCode.SharpZipLib.Zip;


namespace smART.MVC.Present {

  public class FilelHelper {

    public void SaveFile(HttpPostedFileBase file, string destinationPath) {
      CreateDirectory(destinationPath);
      var fileName = Path.GetFileName(file.FileName);
      var filePath = Path.Combine(destinationPath, fileName);
      file.SaveAs(filePath);
    }

    /// <summary>
    /// Save bytes array into file on a given location.
    /// </summary>
    /// <remarks>
    /// If given file path does not exists then it creates a new directory for a given location.
    /// </remarks>
    /// <param name="fileFullPath">Target file path.</param>
    /// <param name="bytes">bytes array of file.</param>
    public  void SaveBytesToFile(Byte[] bytes, string fileFullPath) {
      // If bytes array is null or length is zero.
      //if (bytes == null || bytes.Length == 0) throw new FileNotFoundException("The attached file does not exist at '{0}' this location.");


      // If target directory not exists.
      string targetDir = System.IO.Path.GetDirectoryName(fileFullPath);
      CreateDirectory(targetDir);

      // Save bytes into file.
      using (FileStream fs = new FileStream(fileFullPath, FileMode.CreateNew)) {
        fs.Write(bytes, 0, bytes.Length);
      }
    }


    public void RemoveFile(string fileFullName, string destinationPath) {
      CreateDirectory(destinationPath);
      var fileName = Path.GetFileName(fileFullName);
      var filePath = Path.Combine(destinationPath, fileName);
      DeleteFile(filePath);
      DeleteDirectory(destinationPath);
    }

    public void MoveFile(string fileName, string sourcePath, string destinationPath) {
      if (FileExits(Path.Combine(sourcePath, fileName))) {
        CreateDirectory(destinationPath);
        File.Move(Path.Combine(sourcePath, fileName), Path.Combine(destinationPath, fileName));
        DeleteDirectory(sourcePath);
      }

    }

    public string GetFilePath(string dirPath) {
      string filePath = string.Empty;

      if (DirectoryExits(dirPath)) {

        string[] filePaths = Directory.GetFiles(dirPath);
        if (filePaths != null && filePaths.Count() > 0) {
          filePath = filePaths[0];
        }
      }

      return filePath;
    }

    public string GetFilePathByFileRefId(string fileRefId) {
      string filePath = string.Empty;
      if (!string.IsNullOrEmpty(fileRefId) && Guid.Empty.ToString() != fileRefId) {
        string sourceDir = Path.Combine(Configuration.GetsmARTDocPath(), fileRefId);
        filePath = GetFilePath(sourceDir);

      }

      return filePath;
    }

    public string GetTempFilePathByFileRefId(string fileRefId) {
      string filePath = string.Empty;
      if (!string.IsNullOrEmpty(fileRefId) && Guid.Empty.ToString() != fileRefId) {
        string sourceDir = Path.Combine(Configuration.GetsmARTTempDocPath(), fileRefId);
        filePath = GetFilePath(sourceDir);

      }

      return filePath;
    }


    public string GetSourceDirByFileRefId(string fileRefId) {
      string sourceDirPath = string.Empty;
      if (!string.IsNullOrEmpty(fileRefId) && Guid.Empty.ToString() != fileRefId) {
        sourceDirPath = Path.Combine(Configuration.GetsmARTDocPath(), fileRefId);

      }

      return sourceDirPath;
    }

    public string GetTempSourceDirByFileRefId(string fileRefId) {
      string sourceDirPath = string.Empty;
      if (!string.IsNullOrEmpty(fileRefId) && Guid.Empty.ToString() != fileRefId) {
        sourceDirPath = Path.Combine(Configuration.GetsmARTTempDocPath(), fileRefId);

      }

      return sourceDirPath;
    }

    public void CreateDirectory(string dirPath) {
      if (!DirectoryExits(dirPath)) {
        Directory.CreateDirectory(dirPath);
      }
    }

    public void DeleteParentDir(string dirPath) {
      string parentDirPath = GetParentDirectory(dirPath);

    }

    public void DeleteDirectory(string dirPath) {      
      if (DirectoryExits(dirPath)) {
        DeleteFiles(dirPath);

        Directory.Delete(dirPath);
      }
    }

    public string GetParentDirectory(string directoryPath) {
      string parentDirPath = string.Empty;

      if (DirectoryExits(directoryPath)) {
        parentDirPath = Directory.GetParent(directoryPath).FullName;
      }

      return parentDirPath;
    }


    public void DeleteFiles(string DirPath) {
      try {
        Array.ForEach(Directory.GetFiles(DirPath),
          delegate(string path) { File.Delete(path); });
      }
      catch (Exception) { }
    }


    public void DeleteFile(string filePath) {
      try {
        if (FileExits(filePath)) {
          File.Delete(filePath);
        }
      }
      catch (Exception) { }
    }

    public bool FileExits(string filePath) {
      return File.Exists(filePath);
    }

    public bool DirectoryExits(string directoryPath) {
      return Directory.Exists(directoryPath);
    }

    public FileInfo GetFileInfo(string filePath) {

      System.IO.FileInfo fileInfo = new System.IO.FileInfo(filePath);

      return fileInfo;

    }

    public void CreateZip(string folderToZip, string targetFile) {
      ZipPath(targetFile, folderToZip, null, false, null);
    }

    public void ZipPath(string zipFilePath, string sourceDir, string pattern, bool withSubdirs, string password) {
      FastZip fz = new FastZip();
      fz.CreateZip(zipFilePath, sourceDir, withSubdirs, pattern);
      fz = null;
    }

    public byte[] GetBytesFromFile(string filePath) {
      if (!File.Exists(filePath))
        return null;     
      byte[] imgData = System.IO.File.ReadAllBytes(filePath);
      return imgData;
    }

  }

  public class FileDetails {
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public string FielExt { get; set; }
    public long FileSize { get; set; }
    public string Mime_Type { get; set; }

  }
}