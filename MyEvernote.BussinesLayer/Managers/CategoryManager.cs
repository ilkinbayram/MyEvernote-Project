using MyEvernote.DataAccessLayer.EntityFramework;
using MyEvernote.EntitiesLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.WebPages;

namespace MyEvernote.BussinesLayer.Managers
{
    public class CategoryManager : ManagerBase<Category>
    {
        //private NoteManager _noteM = new NoteManager();
        //private CommentManager _commentM = new CommentManager();
        //private LikeManager _likeM = new LikeManager();
        //public override int Delete(Category category)
        //{
        //    foreach (Note note in category.Notes.ToList())
        //    {
        //        foreach (Liked liked in note.Likes.ToList())
        //        {
        //            _likeM.Delete(liked);
        //        }

        //        foreach (Comment comment in note.Comments.ToList())
        //        {
        //            _commentM.Delete(comment);
        //        }
        //        _noteM.Delete(note);
        //    }
        //    return base.Delete(category);
        //}
    }
}
