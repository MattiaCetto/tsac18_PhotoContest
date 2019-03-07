using System;
using System.Collections.Generic;
using System.Text;
using tsac18_CettolinPhotoContest.data.Models;

namespace tsac18_CettolinPhotoContest.data
{
    public interface IDataAccess
    {
        void LoadImageS3(string url, string UserId);
      IEnumerable <Photo> GetPhoto(string UserId);
        void Vote(int id_p, string id_u, int v); //utilizzando l'handler fare dalla pagina principale con dettaglio 
        
    }
}
