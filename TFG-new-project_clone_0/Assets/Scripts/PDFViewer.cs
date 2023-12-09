using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
//using PdfiumViewer;

public class PDFViewer : MonoBehaviour{
    public GameObject blackboard;
    //[DllImport("Pdfium")] private static extern IntPtr LoadPdfDocument(string filePath);
    //[DllImport("Pdfium")] private static extern int GetPageCount(IntPtr document);
    //[DllImport("Pdfium")] private static extern IntPtr RenderPageToBitmap(IntPtr document, int pageNumber);
    //[DllImport("Pdfium")] private static extern void ClosePdfDocument(IntPtr document);

    // Convertir PDF a array de texturas
    public static Texture2D[] ConvertPdfToImages(string pdfPath){
        /*
        IntPtr document = LoadPdfDocument(pdfPath);

        if (document == IntPtr.Zero){
            Debug.LogError("Error al cargar el documento PDF");
            return null;
        }

        int pageCount = GetPageCount(document);
        List<Texture2D> pageTextures = new List<Texture2D>();

        for (int i = 0; i < pageCount; i++){
            IntPtr pageBitmap = RenderPageToBitmap(document, i);
            // Aquí conviertes el bitmap a una textura de Unity (Texture2D)
            // Crea la textura y copia los datos del bitmap al formato de textura

            // Agrega la textura al array de texturas
            // pageTextures.Add(texture);
        }

        ClosePdfDocument(document);
        */
        return null;//pageTextures.ToArray();
    }


    void Update(){
        if(Input.GetKeyDown(KeyCode.L)){
            //exportPDF("protocoloDeCorrecciones","C:/Users/ssant/Downloads/");
        }
    }

    void exportPDF(string filename,string path){
        /*
        using (var document = PdfDocument.Load($"{path}{filename}.pdf")){
            using (var renderer = new PdfRenderer(document)){
                for (int i = 0; i < document.PageCount; i++){
                    using (var bitmap = renderer.RenderPageToBitmap(i, 300, 300, 96, 96)){
                        bitmap.Save($"{path}{filename}/{filename}_{i}.png", ImageFormat.Png);
                    }
                }
            }
        }
        Debug.Log($"Images saved at {path}{filename}/");
        */
    }
}
