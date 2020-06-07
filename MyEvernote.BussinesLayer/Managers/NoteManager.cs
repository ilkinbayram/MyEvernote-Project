using MyEvernote.BussinesLayer.Tools;
using MyEvernote.DataAccessLayer.EntityFramework;
using MyEvernote.EntitiesLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.BussinesLayer.Managers
{
    public class NoteManager : ManagerBase<Note>
    {
        public new BussinessResult<Note> Update(Note note)
        {
            BussinessResult<Note> bsResNote = new BussinessResult<Note>();
            bsResNote.Result = note;
            if (base.Update(note) > 0)
            {
                bsResNote.Successes = new List<BussinessError>()
                {
                    new BussinessError
                    {
                        AlertColor = "success",
                        ErrorCode = InformingOrError.InfoUpdadetPost,
                        Detail = "Post Ugurla Update Olundu"
                    }
                };
            }
            else
            {
                bsResNote.Errors = new List<BussinessError>()
                {
                    new BussinessError
                    {
                        AlertColor = "danger",
                        ErrorCode = InformingOrError.ErrorNotUpdadetPost,
                        Detail = "Post Update Olunmadi. Zehmet Olmasa Birazdan Tekrar Cehd Edin"
                    }
                };
            }
            return bsResNote;
        }
        public new BussinessResult<Note> Delete(Note note)
        {
            BussinessResult<Note> bsResNote = new BussinessResult<Note>();
            bsResNote.Result = note;
            if (base.Delete(note) <= 0)
            {
                bsResNote.Errors = new List<BussinessError>
                {
                    new BussinessError
                    {
                        AlertColor = "danger",
                        ErrorCode = InformingOrError.ErrorNotDeletedNote,
                        Detail = "Post Silinme Prosesi Ugursuz Oldu. Zehmet Olmasa Birazdan Tekrar Cehd Edin."
                    }
                };
            }
            else
            {
                bsResNote.Successes = new List<BussinessError>
                {
                    new BussinessError
                    {
                        AlertColor = "success",
                        ErrorCode = InformingOrError.InfoDeletedNoteFinished,
                        Detail = $"{note.NoteTitle} Basliqli Post Ugurla Silindi."
                    }
                };
            }
            return bsResNote;
        }

        public new BussinessResult<Note> Insert(Note note)
        {
            BussinessResult<Note> bsResNote = new BussinessResult<Note>();
            bsResNote.Result = note;
            if (base.Insert(note) > 0)
            {
                bsResNote.Successes = new List<BussinessError>
                {
                    new BussinessError
                    {
                        AlertColor = "success",
                        ErrorCode = InformingOrError.InfoNoteInsertedSuccess,
                        Detail = "Yeni Post Ugurla Elave Olundu."
                    }
                };
            }
            else
            {
                bsResNote.Errors = new List<BussinessError>
                {
                    new BussinessError
                    {
                        AlertColor = "danger",
                        ErrorCode = InformingOrError.ErrorNoteInsertedFailed,
                        Detail = "Teessufler olsun ki, Yeni Post Elave Olunarken Xeta Yarandi. Zehmet Olmasa Birazdan Tekrar Cehd Edin."
                    }
                };
            }

            return bsResNote;
        }
    }
}
