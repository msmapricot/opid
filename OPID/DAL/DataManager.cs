using MSM.Models;
using MSM.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Excel;
using System.Data;
using OPIDEntities;

namespace MSM.DAL
{
    public class DataManager
    {
        private static bool firstCall = true;
        private static List<int> incidentals;

        private static List<Check> unmatchedChecks;
        private static List<Check> resolvedChecks;
        private static List<Check> typoChecks;

        public static void Init()
        {
            if (firstCall)
            {
                typoChecks = new List<Check>();
                resolvedChecks = new List<Check>();
                firstCall = false;
            }

            unmatchedChecks = new List<Check>();
            incidentals = new List<int>();
        }

        public static List<DispositionRow> GetResearchRows(string apFileName, string apFileType)
        {
            // List<DispositionRow> originalRows = new List<DispositionRow>();
            //  string pathToApricotReportFile = System.Web.HttpContext.Current.Request.MapPath(string.Format("~/App_Data/Public/{0}.{1}", apFileName, apFileType));
            string pathToResearchReportFile = System.Web.HttpContext.Current.Request.MapPath(string.Format("~/Uploads/{0}.{1}", apFileName, apFileType));

            List<DispositionRow> resRows = ExcelDataReader.GetResearchRows(pathToResearchReportFile);

            /*
            var apricotReportFile = Linq2Excel.GetFactory(pathToApricotReportFile);
            Linq2Excel.PrepareApricotMapping(apricotReportFile);

            var apricotRows = from d in apricotReportFile.Worksheet<DispositionRow>("Sheet1") select d;

            foreach (DispositionRow d in apricotRows)
            {
                originalRows.Add(d);
            }
            */

            return resRows;
        }

        public static List<ModificationRow> GetModificationRows(string apFileName, string apFileType)
        {
            string pathToModificationsReportFile = System.Web.HttpContext.Current.Request.MapPath(string.Format("~/Uploads/{0}.{1}", apFileName, apFileType));
            List<ModificationRow> modRows = ExcelDataReader.GetModificationRows(pathToModificationsReportFile);

            return modRows;
        }

        public static List<Check> GetVoidedChecks(string vcFileName, string vcFileType)
        {
            if (vcFileName.Equals("unknown"))
            {
                // Return an emmpty list of checks.
                return new List<Check>();
            }

            //List<Check> voidedChecks = new List<Check>();
            string pathToVoidedChecksFile = System.Web.HttpContext.Current.Request.MapPath(string.Format("~/Uploads/{0}.{1}", vcFileName, vcFileType));

            List<Check> voidedChecks = ExcelDataReader.GetVoidedChecks(pathToVoidedChecksFile);

            foreach (Check check in voidedChecks)
            {
                // Implicit status of voided checks is "Voided"
                check.Clr = "Voided";
            }

            /*
            var voidedChecksFile = Linq2Excel.GetFactory(pathToVoidedChecksFile);
            var vChecks = from vc in voidedChecksFile.Worksheet<Check>("Sheet1") select vc;

            foreach(Check check in vChecks)
            {
                voidedChecks.Add(check);
            }
            */

            return voidedChecks;
        }

        public static List<Check> GetQuickbooksChecks(string qbFileName, string qbFileType)
        {
            if (qbFileName.Equals("unknown"))
            {
                // Return an empty list of checks.
                return new List<Check>();
            }

            // List<Check> quickbooksChecks = new List<Check>();
            string pathToQuickbooksFile = System.Web.HttpContext.Current.Request.MapPath(string.Format("~/Uploads/{0}.{1}", qbFileName, qbFileType));

            List<Check> quickbooksChecks = ExcelDataReader.GetQuickbooksChecks(pathToQuickbooksFile);

            foreach (Check check in quickbooksChecks)
            {
                // Special rule for checks in Quickbooks: If a check is marked cleared in the amount
                // of 0.00, then it is actually a voided check.
                /*
                if (DataManager.GetDispositionFromCheck(check).Equals("Cleared") && check.Amount.Equals("0"))
                {
                    check.Clr = "Voided";
                }
                */
            }

            /*
            var quickbooksFile = Linq2Excel.GetFactory(pathToQuickbooksFile);
            var vChecks = from vc in quickbooksFile.Worksheet<Check>("Sheet1") select vc;

            foreach (Check check in vChecks)
            {
                quickbookChecks.Add(check);
            }
            */

            return quickbooksChecks;
        }

        private static bool IsVoidedOrCleared(string disposition)
        {
            return !string.IsNullOrEmpty(disposition) && (disposition.Equals("Voided") || disposition.Equals("Cleared"));
        }

        public static bool IsProtectedCheck(string disposition)
        {
            if (string.IsNullOrEmpty(disposition))
            {
                return false;
            }

            return disposition.Equals("Voided/Replaced")
                || disposition.Equals("Voided/Reissued")
                || disposition.Equals("Voided/No Reissue")
                || disposition.Equals("Voided/Reissue Other");
        }

        public static bool IsMistakenlyResolved(string disposition)
        {
            return
                (disposition.Equals("Mistakenly Resolved"));
        }

        private static bool IsResearchCheck(string disposition)
        {
            if (string.IsNullOrEmpty(disposition))
            {
                return true;  // PLB 4/19/2018 was false;
            }

            return (string.IsNullOrEmpty(disposition)
                || disposition.Equals("Voided/Replaced")
                || disposition.Equals("Voided/Reissued")
                || disposition.Equals("Voided/No Reissue")
                || disposition.Equals("Voided/Reissue Other"));
        } 
        private static List<Check> DetermineUnmatchedChecks(List<DispositionRow> researchRows)
        {
            foreach (DispositionRow row in researchRows)
            {
                if (row.LBVDCheckNum != 0 && IsResearchCheck(row.LBVDCheckDisposition))
                {
                    NewUnmatchedCheck(row, "LBVD", row.LBVDCheckDisposition);
                }

                if (row.LBVDCheckNum2 != 0  && IsResearchCheck(row.LBVDCheck2Disposition))
                {
                    NewUnmatchedCheck(row, "LBVD2", row.LBVDCheck2Disposition);
                }

                if (row.LBVDCheckNum3 != 0 && IsResearchCheck(row.LBVDCheck3Disposition))
                {
                    NewUnmatchedCheck(row, "LBVD3", row.LBVDCheck3Disposition);
                }

                if (row.TIDCheckNum != 0 && IsResearchCheck(row.TIDCheckDisposition))
                {
                    if (!string.IsNullOrEmpty(row.TIDCheckDisposition))
                    {
                        int z;
                        z = 2;
                    }
                    NewUnmatchedCheck(row, "TID", row.TIDCheckDisposition);
                }

                if (row.TIDCheckNum2 != 0  && IsResearchCheck(row.TIDCheck2Disposition))
                {
                    NewUnmatchedCheck(row, "TID2", row.TIDCheck2Disposition);
                }

                if (row.TIDCheckNum3 != 0 && IsResearchCheck(row.TIDCheck3Disposition))
                {
                    NewUnmatchedCheck(row, "TID3", row.TIDCheck3Disposition);
                }

                if (row.TDLCheckNum != 0 && IsResearchCheck(row.TDLCheckDisposition))
                {
                    NewUnmatchedCheck(row, "TDL", row.TDLCheckDisposition);
                }

                if (row.TDLCheckNum2 != 0 && IsResearchCheck(row.TDLCheck2Disposition))
                {
                    NewUnmatchedCheck(row, "TDL2", row.TDLCheck2Disposition);
                }

                if (row.TDLCheckNum3 != 0 && IsResearchCheck(row.TDLCheck3Disposition))
                {
                    NewUnmatchedCheck(row, "TDL3", row.TDLCheck3Disposition);
                }

                if (row.MBVDCheckNum != 0 && IsResearchCheck(row.MBVDCheckDisposition))
                {
                    NewUnmatchedCheck(row, "MBVD", row.MBVDCheckDisposition);
                }

                if (row.MBVDCheckNum2 != 0 && IsResearchCheck(row.MBVDCheck2Disposition))
                {
                    NewUnmatchedCheck(row, "MBVD2", row.MBVDCheck2Disposition);
                }

                if (row.MBVDCheckNum3 != 0 && IsResearchCheck(row.MBVDCheck3Disposition))
                {
                    NewUnmatchedCheck(row, "MBVD3", row.MBVDCheck3Disposition);
                }

                if (row.SDCheckNum != 0 && IsResearchCheck(row.SDCheckDisposition))
                {
                    NewUnmatchedCheck(row, "SD", row.SDCheckDisposition);
                }
            }

            return unmatchedChecks;
        }

        private static List<Check> DetermineUnmatchedChecks(List<ModificationRow> modificationRows)
        {
            foreach (ModificationRow row in modificationRows)
            {
                if (row.LBVDCheckNum != 0 && (string.IsNullOrEmpty(row.LBVDCheckDisposition) || row.LBVDCheckDisposition.Equals("Stale Check")))
                {
                    NewUnmatchedCheck(row, "LBVD");
                }

                if (row.TIDCheckNum != 0 && (string.IsNullOrEmpty(row.TIDCheckDisposition) || row.TIDCheckDisposition.Equals("Stale Check")))
                {
                    NewUnmatchedCheck(row, "TID");
                }

                if (row.TDLCheckNum != 0 && (string.IsNullOrEmpty(row.TDLCheckDisposition) || row.TDLCheckDisposition.Equals("Stale Check")))
                {
                    NewUnmatchedCheck(row, "TDL");
                }

                if (row.MBVDCheckNum != 0 && (string.IsNullOrEmpty(row.MBVDCheckDisposition) || row.MBVDCheckDisposition.Equals("Stale Check")))
                {
                    NewUnmatchedCheck(row, "MBVD");
                }

                if (row.SDCheckNum != 0 && (string.IsNullOrEmpty(row.SDCheckDisposition) || row.SDCheckDisposition.Equals("Stale Check")))
                {
                    NewUnmatchedCheck(row, "SD");
                }
            }

            return unmatchedChecks;
        }
        public static void PersistUnmatchedChecks(List<DispositionRow> researchRows)
        {
            List<Check> unmatchedChecks = DetermineUnmatchedChecks(researchRows);
            AppendToUnresolvedChecks(unmatchedChecks);
        }

        public static void PersistUnmatchedChecks(List<ModificationRow> modificationRows)
        {
            List<Check> unmatchedChecks = DetermineUnmatchedChecks(modificationRows);
            AppendToUnresolvedChecks(unmatchedChecks);
        }

        public static void PersistUnresolvedChecks(List<Check> unresolvedChecks)
        {
            AppendToUnresolvedChecks(unresolvedChecks);
        }

        private static bool IsTypo(int checkNum)
        {
            var tc = typoChecks.Find(c => c.Num == checkNum);
            return tc != null;
        }

        private static bool IsIncidental(int checkNum)
        {
            return incidentals.Contains(checkNum);
        }

        private static bool IsResolved(int checkNum)
        {
            var rc = resolvedChecks.Find(c => c.Num == checkNum);
            return rc != null;
        }

        /*
        public static void MarkTypoChecks()
        {
            using (var dbCtx = new MSMEntities())
            {
                var unresolved = dbCtx.Set<ResearchCheck>();

                foreach (ResearchCheck lu in unresolved)
                {
                    if (IsTypo(lu.Num) || IsTypo(-lu.Num))
                    {
                        lu.Matched = true;
                    }
                }

                dbCtx.SaveChanges();
            }
        }

        public static void MarkResolvedChecks()
        {
            using (var dbCtx = new MSMEntities())
            {
                var unresolved = dbCtx.Set<ResearchCheck>();

                foreach (ResearchCheck lu in unresolved)
                {
                    if (IsResolved(lu.Num) || IsResolved(-lu.Num))
                    {
                        lu.Matched = true;
                    }
                }

                dbCtx.SaveChanges();
            }
        }
        */

        public static void MarkResolvedChecks()
        {
            using (OPIDDB opidcontext = new OPIDDB())
            {
                var unresolved = opidcontext.UnresolvedChecks;

                foreach (UnresolvedCheck ucheck in unresolved)
                {
                    if (IsResolved(ucheck.Num) || IsResolved(-ucheck.Num))
                    {
                        ucheck.Matched = true;
                    }
                }

                opidcontext.SaveChanges();
            }
        }

        public static void MarkReResolvedChecks()
        {
            using (OPIDDB opidcontext = new OPIDDB())
            {
                var unresolved = opidcontext.UnresolvedChecks;

                foreach (UnresolvedCheck ucheck in unresolved)
                {
                    if (IsResolved(ucheck.Num) && !ucheck.Disposition.Equals("Mistakenly Resolved")) 
                    {
                        ucheck.Matched = true;
                    }
                }

                opidcontext.SaveChanges();
            }
        }

        public static void MarkTypoChecks()
        {
            using (OPIDDB opidcontext = new OPIDDB())
            {
                var unresolved = opidcontext.UnresolvedChecks;

                foreach (UnresolvedCheck ucheck in unresolved)
                {
                    if (IsTypo(ucheck.Num) || IsTypo(-ucheck.Num))
                    {
                        ucheck.Matched = true;
                    }
                }

                opidcontext.SaveChanges();
            }
        }

        private static bool IsStale(int cnum, List<Check> staleChecks)
        {
            var staleCheck = (staleChecks.Find(check => check.Num == cnum));

            return staleCheck != null;
        }

        public static void MarkStaleChecks(string type)
        {
            List<Check> staleChecks = GetStaleChecks();

            using (var dbCtx = new MSMEntities())
            {
                var longUnmatched = dbCtx.Set<ResearchCheck>();

                foreach (ResearchCheck lu in longUnmatched)
                {
                    if ((type.Equals("interview") && lu.Num > 0) || (type.Equals("modification") && lu.Num < 0))
                    {
                        if (IsStale(lu.Num, staleChecks))
                        {
                            lu.Stale = true;
                        }
                    }
                }

                dbCtx.SaveChanges();
            }
        }

        public static void RemoveTypoChecks()
        {
            MarkTypoChecks();
            DeleteMarkedChecks();
        }

        public static void RemoveResolvedChecks()
        {
            MarkResolvedChecks();
            DeleteMarkedChecks();
        }

        public static void RemoveReResolvedChecks()
        {
            MarkReResolvedChecks();
            DeleteMarkedChecks();
        }

        private static void ResolveIncidentalLBVD(MSM.Models.DataRow row, List<Check> researchChecks, bool findTypos)
        {
            if (row.LBVDCheckNum != 0 && IsVoidedOrCleared(row.LBVDCheckDisposition))
            {
                // Find all checks among the researchChecks which are incidentally resolved by
                // this check number.
                List<Check> incidentalChecks
                    = researchChecks.FindAll(c => (Math.Abs(c.Num) == row.LBVDCheckNum)).ToList();
                bool introducesTypo = false;

                foreach (Check incidentalCheck in incidentalChecks)
                {
                    if (!IsIncidental(incidentalCheck.Num))
                    {
                        introducesTypo = true;
                        incidentals.Add(incidentalCheck.Num);
                        incidentalCheck.Clr = row.LBVDCheckDisposition;
                    }
                }

                if (findTypos && introducesTypo)
                {
                    CreateTypoRootCheck(row, row.LBVDCheckNum, "LBVD", row.LBVDCheckDisposition);
                }
            }

            if (row.LBVDCheckNum2 != 0 && IsVoidedOrCleared(row.LBVDCheck2Disposition))
            {
                // Find all checks among the researchChecks which are incidentally resolved by
                // this check number.
                List<Check> incidentalChecks
                    = researchChecks.FindAll(c => (Math.Abs(c.Num) == row.LBVDCheckNum2)).ToList();
                bool introducesTypo = false;

                foreach (Check incidentalCheck in incidentalChecks)
                {
                    if (!IsIncidental(incidentalCheck.Num))
                    {
                        introducesTypo = true;
                        incidentals.Add(incidentalCheck.Num);
                        incidentalCheck.Clr = row.LBVDCheck2Disposition;
                    }
                }

                if (findTypos && introducesTypo)
                {
                    CreateTypoRootCheck(row, row.LBVDCheckNum2, "LBVD2", row.LBVDCheck2Disposition);
                }
            }

            if (row.LBVDCheckNum3 != 0 && IsVoidedOrCleared(row.LBVDCheck3Disposition))
            {
                // Find all checks among the researchChecks which are incidentally resolved by
                // this check number.
                List<Check> incidentalChecks
                    = researchChecks.FindAll(c => (Math.Abs(c.Num) == row.LBVDCheckNum3)).ToList();
                bool introducesTypo = false;

                foreach (Check incidentalCheck in incidentalChecks)
                {
                    if (!IsIncidental(incidentalCheck.Num))
                    {
                        introducesTypo = true;
                        incidentals.Add(incidentalCheck.Num);
                        incidentalCheck.Clr = row.LBVDCheck3Disposition;
                    }
                }

                if (findTypos && introducesTypo)
                {
                    CreateTypoRootCheck(row, row.LBVDCheckNum3, "LBVD3", row.LBVDCheck3Disposition);
                }
            }
        }

        private static void ResolveIncidentalTID(MSM.Models.DataRow row, List<Check> researchChecks, bool findTypos)
        {
            if (row.TIDCheckNum != 0 && IsVoidedOrCleared(row.TIDCheckDisposition))
            {
                // Find all checks among the researchChecks which are incidentally resolved by
                // this check number.
                List<Check> incidentalChecks = researchChecks.FindAll(c => Math.Abs(c.Num) == row.TIDCheckNum).ToList();
                bool introducesTypo = false;

                foreach (Check incidentalCheck in incidentalChecks)
                {
                    if (!IsIncidental(incidentalCheck.Num))
                    {
                        introducesTypo = true;
                        incidentals.Add(incidentalCheck.Num);
                        incidentalCheck.Clr = row.TIDCheckDisposition;
                    }
                }

                if (findTypos && introducesTypo)
                {
                    CreateTypoRootCheck(row, row.TIDCheckNum, "TID", row.TIDCheckDisposition);
                }
            }

            if (row.TIDCheckNum2 != 0 && IsVoidedOrCleared(row.TIDCheck2Disposition))
            {
                // Find all checks among the researchChecks which are incidentally resolved by
                // this check number.
                List<Check> incidentalChecks = researchChecks.FindAll(c => Math.Abs(c.Num) == row.TIDCheckNum2).ToList();
                bool introducesTypo = false;

                foreach (Check incidentalCheck in incidentalChecks)
                {
                    if (!IsIncidental(incidentalCheck.Num))
                    {
                        introducesTypo = true;
                        incidentals.Add(incidentalCheck.Num);
                        incidentalCheck.Clr = row.TIDCheck2Disposition;
                    }
                }

                if (findTypos && introducesTypo)
                {
                    CreateTypoRootCheck(row, row.TIDCheckNum2, "TID2", row.TIDCheck2Disposition);
                }
            }

            if (row.TIDCheckNum3 != 0 && IsVoidedOrCleared(row.TIDCheck3Disposition))
            {
                // Find all checks among the researchChecks which are incidentally resolved by
                // this check number.
                List<Check> incidentalChecks = researchChecks.FindAll(c => Math.Abs(c.Num) == row.TIDCheckNum3).ToList();
                bool introducesTypo = false;

                foreach (Check incidentalCheck in incidentalChecks)
                {
                    if (!IsIncidental(incidentalCheck.Num))
                    {
                        introducesTypo = true;
                        incidentals.Add(incidentalCheck.Num);
                        incidentalCheck.Clr = row.TIDCheck3Disposition;
                    }
                }

                if (findTypos && introducesTypo)
                {
                    CreateTypoRootCheck(row, row.TIDCheckNum3, "TID3", row.TIDCheck3Disposition);
                }
            }
        }

        private static void ResolveIncidentalTDL(MSM.Models.DataRow row, List<Check> researchChecks, bool findTypos)
        {
            if (row.TDLCheckNum != 0 && IsVoidedOrCleared(row.TDLCheckDisposition))
            {

                // Find all checks among the researchChecks which are incidentally resolved by
                // this check number.
                List<Check> incidentalChecks = researchChecks.FindAll(c => Math.Abs(c.Num) == row.TDLCheckNum).ToList();
                bool introducesTypo = false;

                foreach (Check incidentalCheck in incidentalChecks)
                {
                    if (!IsIncidental(incidentalCheck.Num))
                    {
                        introducesTypo = true;
                        incidentals.Add(incidentalCheck.Num);
                        incidentalCheck.Clr = row.TDLCheckDisposition;
                    }
                }

                if (findTypos && introducesTypo)
                {
                    CreateTypoRootCheck(row, row.TDLCheckNum, "TDL", row.TDLCheckDisposition);
                }
            }

            if (row.TDLCheckNum2 != 0 && IsVoidedOrCleared(row.TDLCheck2Disposition))
            {

                // Find all checks among the researchChecks which are incidentally resolved by
                // this check number.
                List<Check> incidentalChecks = researchChecks.FindAll(c => Math.Abs(c.Num) == row.TDLCheckNum2).ToList();
                bool introducesTypo = false;

                foreach (Check incidentalCheck in incidentalChecks)
                {
                    if (!IsIncidental(incidentalCheck.Num))
                    {
                        introducesTypo = true;
                        incidentals.Add(incidentalCheck.Num);
                        incidentalCheck.Clr = row.TDLCheck2Disposition;
                    }
                }

                if (findTypos && introducesTypo)
                {
                    CreateTypoRootCheck(row, row.TDLCheckNum2, "TDL2", row.TDLCheck2Disposition);
                }
            }

            if (row.TDLCheckNum3 != 0 && IsVoidedOrCleared(row.TDLCheck3Disposition))
            {

                // Find all checks among the researchChecks which are incidentally resolved by
                // this check number.
                List<Check> incidentalChecks = researchChecks.FindAll(c => Math.Abs(c.Num) == row.TDLCheckNum3).ToList();
                bool introducesTypo = false;

                foreach (Check incidentalCheck in incidentalChecks)
                {
                    if (!IsIncidental(incidentalCheck.Num))
                    {
                        introducesTypo = true;
                        incidentals.Add(incidentalCheck.Num);
                        incidentalCheck.Clr = row.TDLCheck3Disposition;
                    }
                }

                if (findTypos && introducesTypo)
                {
                    CreateTypoRootCheck(row, row.TDLCheckNum3, "TDL3", row.TDLCheck3Disposition);
                }
            }
        }

        private static void ResolveIncidentalMBVD(MSM.Models.DataRow row, List<Check> researchChecks, bool findTypos)
        {
            if (row.MBVDCheckNum != 0 && IsVoidedOrCleared(row.MBVDCheckDisposition))
            {
                // Find all checks among the researchChecks which are incidentally resolved by
                // this check number.
                List<Check> incidentalChecks = researchChecks.FindAll(c => Math.Abs(c.Num) == row.MBVDCheckNum).ToList();
                bool introducesTypo = false;

                foreach (Check incidentalCheck in incidentalChecks)
                {
                    if (!IsIncidental(incidentalCheck.Num))
                    {
                        introducesTypo = true;
                        incidentals.Add(incidentalCheck.Num);
                        incidentalCheck.Clr = row.MBVDCheckDisposition;
                    }
                }

                if (findTypos && introducesTypo)
                {
                    CreateTypoRootCheck(row, row.MBVDCheckNum, "MBVD", row.MBVDCheckDisposition);
                }
            }

            if (row.MBVDCheckNum2 != 0 && IsVoidedOrCleared(row.MBVDCheck2Disposition))
            {
                // Find all checks among the researchChecks which are incidentally resolved by
                // this check number.
                List<Check> incidentalChecks = researchChecks.FindAll(c => Math.Abs(c.Num) == row.MBVDCheckNum2).ToList();
                bool introducesTypo = false;

                foreach (Check incidentalCheck in incidentalChecks)
                {
                    if (!IsIncidental(incidentalCheck.Num))
                    {
                        introducesTypo = true;
                        incidentals.Add(incidentalCheck.Num);
                        incidentalCheck.Clr = row.MBVDCheck2Disposition;
                    }
                }

                if (findTypos && introducesTypo)
                {
                    CreateTypoRootCheck(row, row.MBVDCheckNum2, "MBVD2", row.MBVDCheck2Disposition);
                }
            }

            if (row.MBVDCheckNum3 != 0 && IsVoidedOrCleared(row.MBVDCheck3Disposition))
            {
                // Find all checks among the researchChecks which are incidentally resolved by
                // this check number.
                List<Check> incidentalChecks = researchChecks.FindAll(c => Math.Abs(c.Num) == row.MBVDCheckNum3).ToList();
                bool introducesTypo = false;

                foreach (Check incidentalCheck in incidentalChecks)
                {
                    if (!IsIncidental(incidentalCheck.Num))
                    {
                        introducesTypo = true;
                        incidentals.Add(incidentalCheck.Num);
                        incidentalCheck.Clr = row.MBVDCheck3Disposition;
                    }
                }

                if (findTypos && introducesTypo)
                {
                    CreateTypoRootCheck(row, row.MBVDCheckNum3, "MBVD3", row.MBVDCheck3Disposition);
                }
            }
        }

        private static void ResolveIncidentalSD(MSM.Models.DataRow row, List<Check> researchChecks, bool findTypos)
        {
            if (row.SDCheckNum != 0 && IsVoidedOrCleared(row.SDCheckDisposition))
            {
                // Find all checks among the researchChecks which are incidentally resolved by
                // this check number.
                List<Check> incidentalChecks = researchChecks.FindAll(c => Math.Abs(c.Num) == row.SDCheckNum).ToList();
                bool introducesTypo = false;

                foreach (Check incidentalCheck in incidentalChecks)
                {
                    if (!IsIncidental(incidentalCheck.Num))
                    {
                        introducesTypo = true;
                        incidentals.Add(incidentalCheck.Num);
                        incidentalCheck.Clr = row.SDCheckDisposition;
                    }
                }

                if (findTypos && introducesTypo)
                {
                    CreateTypoRootCheck(row, row.SDCheckNum, "SD", row.SDCheckDisposition);
                }
            }
        }

        private static void ResolveIncidentalChecks(List<DispositionRow> researchRows, List<Check> researchChecks, bool findTypos)
        {
            foreach (MSM.Models.DataRow row in researchRows)
            {
                ResolveIncidentalLBVD(row, researchChecks, findTypos);
                ResolveIncidentalTID(row, researchChecks, findTypos);
                ResolveIncidentalTDL(row, researchChecks, findTypos);
                ResolveIncidentalMBVD(row, researchChecks, findTypos);
                ResolveIncidentalSD(row, researchChecks, findTypos);
            }
        }

        private static void ResolveIncidentalChecks(List<ModificationRow> modificationRows, List<Check> researchChecks)
        {
            foreach (MSM.Models.DataRow row in modificationRows)
            {
                ResolveIncidentalLBVD(row, researchChecks, false);
                ResolveIncidentalTID(row, researchChecks, false);
                ResolveIncidentalTDL(row, researchChecks, false);
                ResolveIncidentalMBVD(row, researchChecks, false);
                ResolveIncidentalSD(row, researchChecks, false);
            }
        }

        public static void HandleTypos(List<DispositionRow> researchRows)
        {
            List<Check> unmatchedChecks = DetermineUnmatchedChecks(researchRows);
            ResolveIncidentalChecks(researchRows, unmatchedChecks, true);
            CreateIncidentalTypoChecks(unmatchedChecks);
        }

        public static void HandleIncidentalChecks(List<DispositionRow> researchRows)
        {
            List<Check> researchChecks = GetUnresolvedChecks();
            ResolveIncidentalChecks(researchRows, researchChecks, false);

            // When merging a Research File against the Research Table, a check of
            // known disposition may resolve a check in the Research Table. Find
            // any such check and use it to create a new "incidental" resolved check
            // The status of an incidental resolved check will have been set by the call
            // to method ResolveIncidentalChecks above. If it has not been set, then
            // use "Resolved" as its status. By construction, checks in the Research Table
            // are only those which had no disposition on a Research File that has been used
            // as an input. Hence, only these checks can become new resolved checks.
            CreateIncidentalResolvedChecks(researchChecks);

            // Remove from the Research Table any incidental resolved checks created 
            // by the previous call. 
            RemoveResolvedChecks();
        }

        public static void HandleIncidentalChecks(List<ModificationRow> modificationRows)
        {
            List<Check> researchChecks = GetUnresolvedChecks();
            ResolveIncidentalChecks(modificationRows, researchChecks);

            // When merging a Modifications File against the Research Table, a check of
            // known disposition may resolve a check in the Research Table. Find
            // any such check and use it to create a new "incidental" resolved check
            // The status of an incidental resolved check will have been set by the call
            // to method ResolveIncidentalChecks above. If it has not been set, then
            // use "Resolved" as its status. By construction, checks in the Research Table
            // are only those which had no disposition on a Research File that has been used
            // as an input. Hence, only these checks can become new resolved checks.
            CreateIncidentalResolvedChecks(researchChecks);

            // Remove from the Research Table any incidental resolved checks created 
            // by the previous call. 
            RemoveResolvedChecks();
        }

        private static void CreateIncidentalTypoChecks(List<Check> researchChecks)
        {
            foreach (int cnum in incidentals)
            {
                // Find all the research checks which have the same number as an
                // incidental check.
                List<Check> matchedChecks = researchChecks.FindAll(c => Math.Abs(c.Num) == cnum);

                foreach (Check matchedCheck in matchedChecks)
                {
                    NewTypoCheck(matchedCheck, string.Empty);
                }
            }
        }

        private static void CreateIncidentalResolvedChecks(List<Check> researchChecks)
        {
            foreach (int cnum in incidentals)
            {
                // Find all the research checks which have the same number as an
                // incidental check of
                List<Check> matchedChecks = researchChecks.FindAll(c => c.Num == cnum);

                foreach (Check matchedCheck in matchedChecks)
                {
                    NewResolvedCheck(matchedCheck, (string.IsNullOrEmpty(matchedCheck.Clr) ? "Resolved" : matchedCheck.Clr));
                }
            }
        }

        public static string GetDispositionFromCheck(Check check)
        {
            switch (check.Clr)
            {
                case "C":
                case "Cleared":
                    return "Cleared";
                case "V":
                case "Voided":
                    return "Voided";
                default:
                    if (check.Clr != null && check.Clr[0] == 0xD6)
                    {
                        // Check mark in Quickbooks is character 0xD6
                        return "Cleared";
                    }
                    else if (check.Clr == null)
                    {
                        // Implicitly cleared becaue there is no longer a Clr column on a cleared checks file.
                        return "Cleared";
                    }

                    // Example: Voided/Replaced
                    // Example: Unknown (a Quickbooks check whose Clr column is not checked)
                    return check.Clr;
            }
        }

        public static void NewResolvedCheck(Check check, string disposition)
        {
            check.Clr = disposition;
            resolvedChecks.Add(check);
        }

        private static void CreateTypoRootCheck(MSM.Models.DataRow row, int checkNum, string service, string disposition)
        {
            Check rootCheck = new Check
            {
                Date = row.Date,
                RecordID = row.RecordID,
                InterviewRecordID = row.InterviewRecordID,
                Name = string.Format("{0}, {1}", row.Lname, row.Fname),
                Num = checkNum,
                Service = service,
                Clr = disposition
            };

            typoChecks.Add(rootCheck);
        }

        public static void NewTypoCheck(Check check, string disposition)
        {
            check.Clr = disposition;
            typoChecks.Add(check);
        }

        public static void NewUnmatchedCheck(DispositionRow row, string service, string disposition)
        {
            int checkNum;

            switch (service)
            {
                case "LBVD":
                    checkNum = row.LBVDCheckNum;
                    break;
                case "LBVD2":
                    checkNum = row.LBVDCheckNum2;
                    break;
                case "LBVD3":
                    checkNum = row.LBVDCheckNum3;
                    break;
                case "TID":
                    checkNum = row.TIDCheckNum;
                    break;
                case "TID2":
                    checkNum = row.TIDCheckNum2;
                    break;
                case "TID3":
                    checkNum = row.TIDCheckNum3;
                    break;
                case "TDL":
                    checkNum = row.TDLCheckNum;
                    break;
                case "TDL2":
                    checkNum = row.TDLCheckNum2;
                    break;
                case "TDL3":
                    checkNum = row.TDLCheckNum3;
                    break;
                case "MBVD":
                    checkNum = row.MBVDCheckNum;
                    break;
                case "MBVD2":
                    checkNum = row.MBVDCheckNum2;
                    break;
                case "MBVD3":
                    checkNum = row.MBVDCheckNum3;
                    break;
                case "SD":
                    checkNum = row.SDCheckNum;
                    break;
                default:
                    checkNum = -1;
                    break;
            }

            unmatchedChecks.Add(new Check
            {
                RecordID = row.RecordID,
                InterviewRecordID = row.InterviewRecordID,
                Num = checkNum,
                Name = string.Format("{0}, {1}", row.Lname, row.Fname),
                Date = row.Date,
                Service = service,
                Disposition = disposition
            });
        }

        public static void NewUnmatchedCheck(ModificationRow row, string service)
        {
            int checkNum;

            switch (service)
            {
                case "LBVD":
                    checkNum = -row.LBVDCheckNum;
                    break;
                case "TID":
                    checkNum = -row.TIDCheckNum;
                    break;
                case "TDL":
                    checkNum = -row.TDLCheckNum;
                    break;
                case "MBVD":
                    checkNum = -row.MBVDCheckNum;
                    break;
                case "SD":
                    checkNum = -row.SDCheckNum;
                    break;
                default:
                    checkNum = -1;
                    break;
            }

            unmatchedChecks.Add(new Check
            {
                RecordID = row.RecordID,
                InterviewRecordID = 0,
                Num = checkNum,
                Name = string.Format("{0}, {1}", row.Lname, row.Fname),
                Date = row.Date,
                Service = service
            });
        }

        public static List<Check> GetTypoChecks()
        {
            if (typoChecks == null)
            {
                return new List<Check>();
            }

            return typoChecks;
        }

        public static List<Check> GetResolvedChecks()
        {
            if (resolvedChecks == null)
            {
                return new List<Check>();
            }

            return resolvedChecks;
        }

        private static void UpdateExistingImportRow(Check resolvedCheck, string disposition, ImportRow irow)
        {
            int checkNum = resolvedCheck.Num;

            switch (resolvedCheck.Service)
            {
                case "LBVD":
                    if (irow.LBVDCheckNum == 0)
                    {
                        irow.LBVDCheckNum = checkNum;
                        irow.LBVDCheckDisposition = disposition;
                    }
                    break;
                case "LBVD2":
                    if (irow.LBVDCheckNum2 == 0)
                    {
                        irow.LBVDCheckNum2 = checkNum;
                        irow.LBVDCheck2Disposition = disposition;
                    }
                    break;
                case "LBVD3":
                    if (irow.LBVDCheckNum3 == 0)
                    {
                        irow.LBVDCheckNum3 = checkNum;
                        irow.LBVDCheck3Disposition = disposition;
                    }
                    break;
                case "TID":
                    if (irow.TIDCheckNum == 0)
                    {
                        irow.TIDCheckNum = checkNum;
                        irow.TIDCheckDisposition = disposition;
                    }
                    break;
                case "TID2":
                    if (irow.TIDCheckNum2 == 0)
                    {
                        irow.TIDCheckNum2 = checkNum;
                        irow.TIDCheck2Disposition = disposition;
                    }
                    break;
                case "TID3":
                    if (irow.TIDCheckNum3 == 0)
                    {
                        irow.TIDCheckNum3 = checkNum;
                        irow.TIDCheck3Disposition = disposition;
                    }
                    break;
                case "TDL":
                    if (irow.TDLCheckNum == 0)
                    {
                        irow.TDLCheckNum = checkNum;
                        irow.TDLCheckDisposition = disposition;
                    }
                    break;
                case "TDL2":
                    if (irow.TDLCheckNum2 == 0)
                    {
                        irow.TDLCheckNum2 = checkNum;
                        irow.TDLCheck2Disposition = disposition;
                    }
                    break;
                case "TDL3":
                    if (irow.TDLCheckNum3 == 0)
                    {
                        irow.TDLCheckNum3 = checkNum;
                        irow.TDLCheck3Disposition = disposition;
                    }
                    break;
                case "MBVD":
                    if (irow.MBVDCheckNum == 0)
                    {
                        irow.MBVDCheckNum = checkNum;
                        irow.MBVDCheckDisposition = disposition;
                    }
                    break;
                case "MBVD2":
                    if (irow.MBVDCheckNum2 == 0)
                    {
                        irow.MBVDCheckNum2 = checkNum;
                        irow.MBVDCheck2Disposition = disposition;
                    }
                    break;
                case "MBVD3":
                    if (irow.MBVDCheckNum3 == 0)
                    {
                        irow.MBVDCheckNum3 = checkNum;
                        irow.MBVDCheck3Disposition = disposition;
                    }
                    break;
                case "SD":
                    if (irow.SDCheckNum == 0)
                    {
                        irow.SDCheckNum = checkNum;
                        irow.SDCheckDisposition = disposition;
                    }
                    break;
                default:
                    break;
            }
        }

        // Called only by FileDownloaderController.DownloadImportMe
        public static List<ImportRow> GetImportRows()
        {
            List<ImportRow> importRows = new List<ImportRow>();

            // Each resolved check creates a new import row or updates an existing one.
            foreach (Check resolvedCheck in resolvedChecks)
            {
                string disposition = resolvedCheck.Clr;   //GetDispositionFromCheck(resolvedCheck);

                if (!disposition.Equals("Unknown"))
                {
                    List<ImportRow> irows = (from irow in importRows
                                             where irow.LBVDCheckNum == resolvedCheck.Num
                                                   || irow.LBVDCheckNum2 == resolvedCheck.Num
                                                   || irow.LBVDCheckNum3 == resolvedCheck.Num
                                                   || irow.TIDCheckNum == resolvedCheck.Num
                                                   || irow.TIDCheckNum2 == resolvedCheck.Num
                                                   || irow.TIDCheckNum3 == resolvedCheck.Num
                                                   || irow.TDLCheckNum == resolvedCheck.Num
                                                   || irow.TDLCheckNum2 == resolvedCheck.Num
                                                   || irow.TDLCheckNum3 == resolvedCheck.Num
                                                   || irow.MBVDCheckNum == resolvedCheck.Num
                                                   || irow.MBVDCheckNum2 == resolvedCheck.Num
                                                   || irow.MBVDCheckNum3 == resolvedCheck.Num
                                                   || irow.SDCheckNum == resolvedCheck.Num

                                                   // Does resolvedCheck match an existing importRow by ID?
                                                   // This is the case where there is more than one check on an import row, IR, 
                                                   // and resolvedCheck will be used to update row IR.
                                                   || (resolvedCheck.InterviewRecordID != 0 && irow.InterviewRecordID == resolvedCheck.InterviewRecordID)
                                                   || (resolvedCheck.RecordID != 0 && irow.RecordID == resolvedCheck.RecordID)
                                             select irow).ToList();

                    if (irows.Count() == 0)
                    {
                        // There is no import row representing this resolved check.
                        // Create one.
                        importRows.Add(NewImportRow(resolvedCheck, disposition));
                    }
                    else
                    {
                        bool added = false;

                        foreach (ImportRow irow in irows)
                        {
                            if ((resolvedCheck.Service == "LBVD" || resolvedCheck.Service == "LBVD2" || resolvedCheck.Service == "LBVD3"
                                || resolvedCheck.Service == "TID" || resolvedCheck.Service == "TID2" || resolvedCheck.Service == "TID3"
                                || resolvedCheck.Service == "TDL" || resolvedCheck.Service == "TDL2" || resolvedCheck.Service == "TDL3"
                                || resolvedCheck.Service == "MBVD" || resolvedCheck.Service == "MBVD2" || resolvedCheck.Service == "MBVD3")
                                &&
                                ((resolvedCheck.InterviewRecordID != 0 && resolvedCheck.InterviewRecordID != irow.InterviewRecordID)
                                ||
                                (resolvedCheck.RecordID != 0 && resolvedCheck.RecordID != irow.RecordID)))
                            {
                                // Case of same check number being used for multiple
                                // birth certificates.
                                if (!added)
                                {
                                    importRows.Add(NewImportRow(resolvedCheck, disposition));
                                    // Prevent the same resolved check from being added twice.
                                    added = true;
                                }
                            }
                            else
                            {
                                // Found row among existing import rows. There is more than one check
                                // number on this row. In other words, the client had more than
                                // one check written for the visit this row corresponds to.
                                UpdateExistingImportRow(resolvedCheck, disposition, irow);
                            }
                        }
                    }
                }
            }

            return importRows;
        }

        private static ImportRow NewImportRow(Check resolvedCheck, string disposition)
        {
            ImportRow importRow = new ImportRow
            {
                RecordID = resolvedCheck.RecordID,
                InterviewRecordID = resolvedCheck.InterviewRecordID,
                LBVDCheckNum = (resolvedCheck.Service.Equals("LBVD") ? resolvedCheck.Num : 0),
                LBVDCheckDisposition = (resolvedCheck.Service.Equals("LBVD") ? disposition : ""),
                LBVDCheckNum2 = (resolvedCheck.Service.Equals("LBVD2") ? resolvedCheck.Num : 0),
                LBVDCheck2Disposition = (resolvedCheck.Service.Equals("LBVD2") ? disposition : ""),
                LBVDCheckNum3 = (resolvedCheck.Service.Equals("LBVD3") ? resolvedCheck.Num : 0),
                LBVDCheck3Disposition = (resolvedCheck.Service.Equals("LBVD3") ? disposition : ""),
                TIDCheckNum = (resolvedCheck.Service.Equals("TID") ? resolvedCheck.Num : 0),
                TIDCheckDisposition = (resolvedCheck.Service.Equals("TID") ? disposition : ""),
                TIDCheckNum2 = (resolvedCheck.Service.Equals("TID2") ? resolvedCheck.Num : 0),
                TIDCheck2Disposition = (resolvedCheck.Service.Equals("TID2") ? disposition : ""),
                TIDCheckNum3 = (resolvedCheck.Service.Equals("TID3") ? resolvedCheck.Num : 0),
                TIDCheck3Disposition = (resolvedCheck.Service.Equals("TID3") ? disposition : ""),
                TDLCheckNum = (resolvedCheck.Service.Equals("TDL") ? resolvedCheck.Num : 0),
                TDLCheckDisposition = (resolvedCheck.Service.Equals("TDL") ? disposition : ""),
                TDLCheckNum2 = (resolvedCheck.Service.Equals("TDL2") ? resolvedCheck.Num : 0),
                TDLCheck2Disposition = (resolvedCheck.Service.Equals("TDL2") ? disposition : ""),
                TDLCheckNum3 = (resolvedCheck.Service.Equals("TDL3") ? resolvedCheck.Num : 0),
                TDLCheck3Disposition = (resolvedCheck.Service.Equals("TDL3") ? disposition : ""),
                MBVDCheckNum = (resolvedCheck.Service.Equals("MBVD") ? resolvedCheck.Num : 0),
                MBVDCheckDisposition = (resolvedCheck.Service.Equals("MBVD") ? disposition : ""),
                MBVDCheckNum2 = (resolvedCheck.Service.Equals("MBVD2") ? resolvedCheck.Num : 0),
                MBVDCheck2Disposition = (resolvedCheck.Service.Equals("MBVD2") ? disposition : ""),
                MBVDCheckNum3 = (resolvedCheck.Service.Equals("MBVD3") ? resolvedCheck.Num : 0),
                MBVDCheck3Disposition = (resolvedCheck.Service.Equals("MBVD3") ? disposition : ""),
                SDCheckNum = (resolvedCheck.Service.Equals("SD") ? resolvedCheck.Num : 0),
                SDCheckDisposition = (resolvedCheck.Service.Equals("SD") ? disposition : "")
            };

            return importRow;
        }

        public static List<Check> GetStaleChecks()
        {
            DateTime today = DateTime.Now;

            List<Check> staleChecks = new List<Check>();

            List<Check> researchChecks = GetResearchChecks();

            foreach (Check check in researchChecks)
            {
                TimeSpan elapsed = today.Subtract(check.Date);

                if (elapsed.TotalDays > 30) // && check.Stale != true) // don't return a check already marked stale
                {
                    staleChecks.Add(check);
                }
            }

            return staleChecks;
        }

        public static List<ImportRow> GetStaleRows(List<Check> staleChecks)
        {
            List<ImportRow> importRows = new List<ImportRow>();

            // Each stale check creates a new import row or updates an existing one.
            foreach (Check staleCheck in staleChecks)
            {
                List<ImportRow> irows = (from irow in importRows
                                         where irow.LBVDCheckNum == staleCheck.Num
                                               || irow.TIDCheckNum == staleCheck.Num
                                               || irow.TDLCheckNum == staleCheck.Num
                                               || irow.MBVDCheckNum == staleCheck.Num
                                               || irow.SDCheckNum == staleCheck.Num

                                               // Does resolvedCheck match an existing importRow by ID?
                                               // This is the case where there is more than one check on an import row, IR, 
                                               // and resolvedCheck will be used to update row IR.
                                               || (staleCheck.InterviewRecordID != 0 && irow.InterviewRecordID == staleCheck.InterviewRecordID)
                                               || (staleCheck.RecordID != 0 && irow.RecordID == staleCheck.RecordID)
                                         select irow).ToList();

                if (irows.Count() == 0)
                {
                    // There is no import row representing this resolved check.
                    // Create one.
                    importRows.Add(NewImportRow(staleCheck, "Stale Check"));
                }
                else
                {
                    bool added = false;

                    foreach (ImportRow irow in irows)
                    {
                        if ((staleCheck.Service == "LBVD" || staleCheck.Service == "MBVD")
                            &&
                            ((staleCheck.InterviewRecordID != 0 && staleCheck.InterviewRecordID != irow.InterviewRecordID)
                            ||
                            (staleCheck.RecordID != 0 && staleCheck.RecordID != irow.RecordID)))
                        {
                            // Case of same check number being used for multiple
                            // birth certificates.
                            if (!added)
                            {
                                importRows.Add(NewImportRow(staleCheck, "Stale Check"));
                                // Prevent the same resolved check from being added twice.
                                added = true;
                            }
                        }
                        else
                        {
                            // Found row among existing import rows. There is more than one check
                            // number on this row. In other words, the client had more than
                            // one check written for the visit this row corresponds to.
                            UpdateExistingImportRow(staleCheck, "Stale Check", irow);
                        }
                    }
                }
            }

            return importRows;
        }

        public static List<Check> GetResearchChecks()
        {
            List<Check> researchChecks = new List<Check>();

            using (var dbCtx = new MSMEntities())
            {
                var unresolved = dbCtx.Set<ResearchCheck>();

                foreach (var lu in unresolved)
                {
                    researchChecks.Add(new Check
                    {
                        RecordID = lu.RecordID,
                        InterviewRecordID = lu.InterviewRecordID,
                        Num = lu.Num,
                        Name = lu.Name,
                        Date = lu.Date,
                        Service = lu.Service,
                        Disposition = lu.Disposition,
                        Matched = lu.Matched,
                        Stale = lu.Stale
                    });
                }
            }

            //  List<Check> zeros = researchChecks.FindAll(c => c.InterviewRecordID == 0);

            return researchChecks;
        }

        public static List<Check> GetUnresolvedChecks()
        {
            List<Check> unresolvedChecks = new List<Check>();

            using (OPIDDB opidcontext  = new OPIDDB())
            {
                List<UnresolvedCheck> uchecks = opidcontext.UnresolvedChecks.Select(u => u).ToList();

                foreach (var lu in uchecks)
                {
                    unresolvedChecks.Add(new Check
                    {
                        RecordID = lu.RecordID,
                        InterviewRecordID = lu.InterviewRecordID,
                        Num = lu.Num,
                        Name = lu.Name,
                        Date = lu.Date,
                        Service = lu.Service,
                        Disposition = lu.Disposition,
                        Matched = lu.Matched
                    });
                }
            }

            //  List<Check> zeros = researchChecks.FindAll(c => c.InterviewRecordID == 0);

            return unresolvedChecks;
        }

        /*
        private static void AppendToResearchChecks(List<Check> checks)
        {
            try
            {
                using (var dbCtx = new MSMEntities())
                {
                    var unresolved = dbCtx.Set<ResearchCheck>();

                    int updateCnt = 0;
                    int preCnt = unresolved.Count();

                    foreach (Check check in checks)
                    {
                        ResearchCheck existing = (from c in unresolved
                                                  where c.Num == check.Num
                                                  select c).FirstOrDefault();

                        // if (existing == null && !IsKnownDisposition(check.Num))
                        if (existing == null) // && string.IsNullOrEmpty(check.Clr))
                        {

                            ResearchCheck unm = new ResearchCheck
                            {
                                RecordID = check.RecordID,
                                InterviewRecordID = check.InterviewRecordID,
                                Num = check.Num,
                                Name = check.Name,
                                Date = check.Date,
                                Service = check.Service,
                                Disposition = "VR", // check.Disposition,
                                Matched = false,
                                Stale = false
                            };

                            if (!string.IsNullOrEmpty(check.Disposition))
                            {
                                int z;
                                z = 2;
                            }
                            //  unresolved.Add(unm);
                            dbCtx.ResearchChecks.Add(unm);
                           // dbCtx.ResearchChecks.Attach(unm);

                            if (unm.Id == 0)
                            {
                                dbCtx.Entry(unm).State = System.Data.Entity.EntityState.Added;
                                int added;
                                added = 2;
                            }
                        }

                        else if (!string.IsNullOrEmpty(check.Disposition))
                        {
                            updateCnt += 1;
                            // The existing check may have its disposition
                            // changed to, for example, Voided/Replaced.
                            // If a file of voided checks contains a check with number existing.Num
                            // then this change of disposition will protect this check from having its status
                            // in Apricot changed from Voided/Replaced to Voided
                            // existing.Disposition = check.Disposition;
                            int z;
                            z = 2;
                        }
                    }

                    dbCtx.SaveChanges();

                    ResearchCheck young = unresolved.Where(u => u.Num == 81173).SingleOrDefault();
                    int aftercnt = unresolved.Count();
                    int zz = 2;
                    zz = 2;

                }
            } catch(Exception e)
            {
                int bug;
                bug = 2;
            }

            using (var dbCtx = new MSMEntities())
            {
                var unresolved = dbCtx.Set<ResearchCheck>();
                ResearchCheck young = unresolved.Where(u => u.Num == 81173).SingleOrDefault();
                //  int aftercnt = unresolved.Count();
                int zz = 2;
                zz = 2;
            }
        }
        */

        private static void AppendToUnresolvedChecks(List<Check> checks)
        {
            try
            {
                using (OPIDDB opidcontext = new OPIDDB())
                {
                    foreach (Check check in checks)
                    {
                        UnresolvedCheck existing = opidcontext.UnresolvedChecks.Where(u => u.Num == check.Num).FirstOrDefault();

                        if (existing == null) // && string.IsNullOrEmpty(check.Clr))
                        {
                            UnresolvedCheck unresolved = new UnresolvedCheck
                            {
                                RecordID = check.RecordID,
                                InterviewRecordID = check.InterviewRecordID,
                                Num = check.Num,
                                Name = check.Name,
                                Date = check.Date,
                                Service = check.Service,
                                Disposition = check.Disposition,
                                Matched = false
                            };

                            opidcontext.UnresolvedChecks.Add(unresolved);   
                        }
                        else if (!string.IsNullOrEmpty(check.Disposition))
                        {
                            // The existing check may have its disposition
                            // changed to, for example, Voided/Replaced.
                            // If a file of voided checks contains a check with number existing.Num
                            // then this change of disposition will protect this check from having its status
                            // in Apricot changed from Voided/Replaced to Voided
                            existing.Disposition = check.Disposition;
                        }
                    }

                    opidcontext.SaveChanges();
                }
            }
            catch (Exception e)
            {
                 
            }
        }

        /*
        private static void DeleteMarkedChecks()
        {
            using (var dbCtx = new MSMEntities())
            {
                dbCtx.ResearchChecks.RemoveRange(dbCtx.ResearchChecks.Where(lu => lu.Matched == true));
                dbCtx.SaveChanges();
            }
        }
        */

        private static void DeleteMarkedChecks()
        {
            using (OPIDDB opidcontext = new OPIDDB())
            {
                List<UnresolvedCheck> resolved = opidcontext.UnresolvedChecks.Where(u => u.Matched == true).ToList();

                foreach (UnresolvedCheck ucheck in resolved)
                {
                    opidcontext.UnresolvedChecks.Remove(ucheck);
                }
                
                opidcontext.SaveChanges();
            }
        }

        public static string ResolveCheck(int checkNum)
        {
            string status;

            using (var dbCtx = new MSMEntities())
            {
                var unresolved = dbCtx.Set<ResearchCheck>();

                var check = (from lu in unresolved
                             where lu.Num == checkNum
                             select lu).FirstOrDefault();

                if (check == null)
                {
                    status = string.Format("<p>Could not find check with number {0} in research table.<p>", checkNum);
                }
                else
                {
                    unresolved.Remove(check);
                    dbCtx.SaveChanges();
                    status = string.Format("<p>Removed from research table:<br/>&nbsp;&nbsp;&nbsp;Date: {0}<br/>&nbsp;&nbsp;&nbsp;Record ID: {1}<br/>&nbsp;&nbsp;&nbsp;Interview Record ID: {2}<br/>&nbsp;&nbsp;&nbsp;Name: {3}<br/>&nbsp;&nbsp;&nbsp;Check number: {4}<br/>&nbsp;&nbsp;&nbsp;Service: {5}</p>", check.Date.ToString("d"), check.RecordID, check.InterviewRecordID, check.Name, check.Num, check.Service);
                }

                return status;
            }
        }

        public static void ProcessMistakenlyResolvedChecks(List<Check> mistakenlyResolved)
        {
            using (OPIDDB opidcontext = new OPIDDB())
            {
                foreach (Check mr in mistakenlyResolved)
                {
                    List<UnresolvedCheck> unresolvedChecks = opidcontext.UnresolvedChecks.Where(u => u.Num == mr.Num).ToList();

                    foreach (UnresolvedCheck ucheck in unresolvedChecks)
                    {
                        ucheck.Disposition = "Mistakenly Resolved";
                    }
                }

                opidcontext.SaveChanges();
            }
        }

        /*
        public static string IsEmpty()
        {
            string status = "full";

            using (var dbCtx = new MSMEntities())
            {
                var unresolved = dbCtx.Set<ResearchCheck>();

                if (unresolved.Count() == 0) // Is the table empty for rebuild?
                {
                    status = "empty";
                }
            }

            return status;
        }

        public static void RestoreResearchTable(string rtFileName, string rtFileType)
        {
            string pathToResearchTableFile = System.Web.HttpContext.Current.Request.MapPath(string.Format("~/Uploads/{0}.{1}", rtFileName, rtFileType));

            List<ResearchCheck> resChecks = ExcelDataReader.GetResearchChecks(pathToResearchTableFile);
            DateTime today = DateTime.Now;

           

            RebuildResearchTable(resChecks);
        }

            
        private static void RebuildResearchTable(List<ResearchCheck> resChecks)
        {
            using (var dbCtx = new MSMEntities())
            {
                var unresolved = dbCtx.Set<ResearchCheck>();

                if (unresolved.Count() == 0) // Is the table empty for rebuild?
                {
                    foreach (ResearchCheck rc in resChecks)
                    {
                        unresolved.Add(rc);
                    }

                    dbCtx.SaveChanges();
                    return;
                }
            }
        }
       */

        public static string IsEmpty()
        {
            string status = "full";

            using (OPIDDB opidcontext = new OPIDDB())
            {
                var unresolved = opidcontext.UnresolvedChecks;

                if (unresolved.Count() == 0) // Is the table empty for rebuild?
                {
                    status = "empty";
                }
            }

            return status;
        }


        public static void RestoreResearchTable(string rtFileName, string rtFileType)
        {
            string pathToResearchTableFile = System.Web.HttpContext.Current.Request.MapPath(string.Format("~/Uploads/{0}.{1}", rtFileName, rtFileType));

            List<UnresolvedCheck> unresolvedChecks = ExcelDataReader.GetUnresolvedChecks(pathToResearchTableFile);
            DateTime today = DateTime.Now;

            // Mark checks over 30 days old as stale.
            /* PLB 10/16/2017 No, don't do this at restore time. A check in the Research Table that is over 30 days old will
             * be marked as stale by the import file created by "Download interview stale checks" on the Research tab. This
             * import file will have a name like
             *    interview-stalechecks-14-Oct-2017-1141.csv
             * When this file is imported all the old checks (> 30 days) in the Research Table will be marked stale. A similar
             * file to import will be created by "Download modifications stale checks" on the Research tab.
             * 
            foreach (ResearchCheck rc in resChecks)
            {
                TimeSpan elapsed = today.Subtract(rc.Date);

                if (elapsed.TotalDays > 30)
                {
                   rc.Stale = true;
                }
                else
                {
                    rc.Stale = false;
                }
            }
            */

            RebuildUnresolvedChecksTable(unresolvedChecks);
        }


        private static void RebuildUnresolvedChecksTable(List<UnresolvedCheck> unresolvedChecks)
        {
            using (OPIDDB opidcontext = new OPIDDB())
            {
                var unresolved = opidcontext.UnresolvedChecks;

                if (unresolved.Count() == 0) // Is the table empty for rebuild?
                {
                    foreach (UnresolvedCheck rc in unresolvedChecks)
                    {
                        unresolved.Add(rc);
                    }

                    opidcontext.SaveChanges();
                    return;
                }
            }
        }
    }
}
 