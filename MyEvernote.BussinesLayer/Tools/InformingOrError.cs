namespace MyEvernote.BussinesLayer.Tools
{
    public enum InformingOrError
    {
        ErrorRegisterAlreadyExistUsername = 101,
        ErrorRegisterAlreadyExistEmail = 102,
        ErrorRegisterUnknownSoftwareCode = 103,
        ErrorRegisterFailedSendingConfirmEmail = 104,
        ErrorLoginIsNotActiveUser = 201,
        ErrorLoginNotExistUsername = 202,
        ErrorLoginNotExistPassword = 203,
        ErrorLoginNotFoundUser = 204,

        InformMailAlreadyConfirmed = 501,
        ErrorBannedUser = 502,
        ErrorFailedUpdateProcess = 503,
        SuccessUpdateEditProfile = 504,
        ErrorNewUserNotSaved = 505,
        InfoNewUserSaved = 506,
        InfoNewNoteSaved = 507,
        ErrorNotUpdadetPost = 508,
        InfoUpdadetPost = 509,
        ErrorNotDeletedNote = 510,
        InfoDeletedNoteFinished = 511,
        InfoNoteInsertedSuccess = 512,
        ErrorNoteInsertedFailed = 513,
        ErrorFailedChangePasswordEmail = 514,
        ErrorAccessDenied = 515,
        ExceptionTurnedError = 516
    }
}
