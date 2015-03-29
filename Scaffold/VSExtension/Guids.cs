using System;

namespace MadsKristensen.AddAnyFile
{
    static class GuidList
    {
        public const string guidAddAnyFilePkgString = "5da9a483-453d-438b-8cf6-8d487a29ef36";
        public const string guidAddAnyFileCmdSetString = "5a56f0bf-a23a-4767-8826-673c115d2e29";

        public static readonly Guid guidAddAnyFileCmdSet = new Guid(guidAddAnyFileCmdSetString);
    }

    static class PkgCmdIDList
    {
        public const uint cmdidMyCommand = 0x100;
    }
}