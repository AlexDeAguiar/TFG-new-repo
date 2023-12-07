using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Pdfium;

public class PDFViewer : MonoBehaviour{
    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        if(Input.GetKeyDown(KeyCode.L)){
            exportPDF("protocoloDeCorrecciones","C:/Users/ssant/Downloads/");
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
