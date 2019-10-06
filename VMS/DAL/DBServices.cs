using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Data;
using VMS.Models;

namespace VMS.DAL
{
    public class DBServices
    {
        public static string GetUserType(Login login)
        {
            String userType = "";
            using (MySqlConnection con = new MySqlConnection(MvcApplication.conStr))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand("GetUserType", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("@pUserId", MySqlDbType.VarChar, 20);
                    cmd.Parameters["@pUserId"].Value = login.UserId;
                    cmd.Parameters.Add("@pPasswd", MySqlDbType.VarChar, 16);
                    cmd.Parameters["@pPasswd"].Value = login.Passwd;
                    cmd.Parameters.Add("@ireturnvalue", MySqlDbType.VarChar, 1);
                    cmd.Parameters["@ireturnvalue"].Direction = ParameterDirection.ReturnValue;
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        rdr.Read();
                        userType = rdr[0].ToString();
                    }
                }
            }
            return userType;
        }

        public static List<TrainerReq> GetTrainerReqs()
        {
            TrainerReq trainerReq;
            List<TrainerReq> trainerReqs = new List<TrainerReq>();

            using (MySqlConnection con = new MySqlConnection(MvcApplication.conStr))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand("GetTrainerReqs", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            trainerReq = new TrainerReq();
                            trainerReq.ReqId = Convert.ToInt32(rdr["ReqId"].ToString());
                            trainerReq.ReqDesc = rdr["ReqDesc"].ToString();
                            trainerReq.CutOffDate = DateTime.Parse(rdr["CutOffDate"].ToString());
                            trainerReqs.Add(trainerReq);
                        }
                    }
                }
            }

            return trainerReqs;
        }

        public static TrainerReq GetTrainerReq(int reqId)
        {
            TrainerReq trainerReq = null;
            using (MySqlConnection con = new MySqlConnection(MvcApplication.conStr))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand("GetTrainerReq", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("@pReqId", MySqlDbType.Int32);
                    cmd.Parameters["@pReqId"].Value = reqId;
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            trainerReq = new TrainerReq();
                            trainerReq.ReqId = Convert.ToInt32(rdr["ReqId"].ToString());
                            trainerReq.ReqDesc = rdr["ReqDesc"].ToString();
                            trainerReq.CutOffDate = DateTime.Parse(rdr["CutOffDate"].ToString());
                        }
                    }
                }
            }
            return trainerReq;
        }

        public static int AddTrainerReq(TrainerReq trainerReq)
        {

            int statusCode = 0;
            using (MySqlConnection con = new MySqlConnection(MvcApplication.conStr))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand("AddTrainerReq", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.Add("@pReqId", MySqlDbType.Int32);
                    cmd.Parameters["@pReqId"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("@pReqDesc", MySqlDbType.VarChar, 100);
                    cmd.Parameters["@pReqDesc"].Value = trainerReq.ReqDesc;

                    cmd.Parameters.Add("@pCutOffDate", MySqlDbType.Date);
                    cmd.Parameters["@pCutOffDate"].Value = trainerReq.CutOffDate;

                    cmd.Parameters.Add("@pStatusCode", MySqlDbType.Int32);
                    cmd.Parameters["@pStatusCode"].Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();
                    statusCode = Convert.ToInt32(cmd.Parameters["@pStatusCode"].Value);
                    if (statusCode == 1)
                    {
                        int reqId = Convert.ToInt32(cmd.Parameters["@pReqId"].Value);
                        trainerReq.ReqId = reqId;
                    }
                }
            }
            return statusCode;
        }

        public static int EditTrainerReq(TrainerReq trainerReq)
        {

            int statusCode = 0;
            using (MySqlConnection con = new MySqlConnection(MvcApplication.conStr))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand("EditTrainerReq", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.Add("@pReqId", MySqlDbType.Int32);
                    cmd.Parameters["@pReqId"].Value = trainerReq.ReqId;

                    cmd.Parameters.Add("@pReqDesc", MySqlDbType.VarChar, 100);
                    cmd.Parameters["@pReqDesc"].Value = trainerReq.ReqDesc;

                    cmd.Parameters.Add("@pCutOffDate", MySqlDbType.Date);
                    cmd.Parameters["@pCutOffDate"].Value = trainerReq.CutOffDate;

                    cmd.Parameters.Add("@pStatusCode", MySqlDbType.Int32);
                    cmd.Parameters["@pStatusCode"].Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();
                    statusCode = Convert.ToInt32(cmd.Parameters["@pStatusCode"].Value);
                    if (statusCode == 1)
                    {
                        int reqId = Convert.ToInt32(cmd.Parameters["@pReqId"].Value);
                        trainerReq.ReqId = reqId;
                    }
                }
            }
            return statusCode;
        }

        public static List<TrainerProfile> GetTrainerProfilesAdmin(int reqId)
        {
            TrainerProfile trainerProfile;
            List<TrainerProfile> trainerProfiles = new List<TrainerProfile>();

            using (MySqlConnection con = new MySqlConnection(MvcApplication.conStr))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand("GetTrainerProfilesAdmin", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("@pReqId", MySqlDbType.Int32);
                    cmd.Parameters["@pReqId"].Value = reqId;
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            trainerProfile = new TrainerProfile();
                            trainerProfile.ReqId = reqId;
                            trainerProfile.ProfileId = Convert.ToInt32(rdr["ProfileId"].ToString());
                            trainerProfile.TrainerName = rdr["TrainerName"].ToString();
                            trainerProfile.VendorId = rdr["VendorId"].ToString();
                            trainerProfile.SMEId = rdr["SMEId"].ToString();
                            trainerProfile.Feedback = rdr["Feedback"].ToString();
                            string rating = rdr["Rating"].ToString();
                            if (!rating.Equals(""))
                            {
                                trainerProfile.Rating = Convert.ToInt32(rdr["Rating"].ToString());
                            } else
                            {
                                trainerProfile.Rating = 0;
                            }

                            trainerProfiles.Add(trainerProfile);
                        }
                    }
                }
            }

            return trainerProfiles;
        }

        public static List<TrainerProfile> GetTrainerProfilesVendor(int reqId,string vendorId)
        {
            TrainerProfile trainerProfile;
            List<TrainerProfile> trainerProfiles = new List<TrainerProfile>();

            using (MySqlConnection con = new MySqlConnection(MvcApplication.conStr))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand("GetTrainerProfilesVendor", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("@pReqId", MySqlDbType.Int32);
                    cmd.Parameters["@pReqId"].Value = reqId;
                    cmd.Parameters.Add("@pVendorId", MySqlDbType.VarChar,20);
                    cmd.Parameters["@pVendorId"].Value = vendorId;
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            trainerProfile = new TrainerProfile();
                            trainerProfile.ProfileId = Convert.ToInt32(rdr["ProfileId"].ToString());
                            trainerProfile.ReqId = reqId;
                            trainerProfile.TrainerName = rdr["TrainerName"].ToString();
                            trainerProfile.VendorId = vendorId;
                            trainerProfiles.Add(trainerProfile);
                        }
                    }
                }
            }

            return trainerProfiles;
        }

        public static int AddTrainerProfile(TrainerProfile trainerProfile)
        {

            int statusCode = 0;
            using (MySqlConnection con = new MySqlConnection(MvcApplication.conStr))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand("AddTrainerProfile", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.Add("@pProfileId", MySqlDbType.Int32);
                    cmd.Parameters["@pProfileId"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("@pReqId", MySqlDbType.Int32);
                    cmd.Parameters["@pReqId"].Value = trainerProfile.ReqId;

                    cmd.Parameters.Add("@pTrainerName", MySqlDbType.VarChar, 50);
                    cmd.Parameters["@pTrainerName"].Value = trainerProfile.TrainerName;

                    cmd.Parameters.Add("@pVendorId", MySqlDbType.VarChar, 20);
                    cmd.Parameters["@pVendorId"].Value = trainerProfile.VendorId;

                    cmd.Parameters.Add("@pStatusCode", MySqlDbType.Int32);
                    cmd.Parameters["@pStatusCode"].Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();
                    statusCode = Convert.ToInt32(cmd.Parameters["@pStatusCode"].Value);
                    if (statusCode == 1)
                    {
                        int profileId = Convert.ToInt32(cmd.Parameters["@pProfileId"].Value);
                        trainerProfile.ProfileId = profileId;
                    }
                }
            }
            return statusCode;
        }

        public static TrainerProfile GetTrainerProfile(int profileId)
        {
            TrainerProfile trainerProfile=null;
            
            using (MySqlConnection con = new MySqlConnection(MvcApplication.conStr))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand("GetTrainerProfile", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("@pProfileId", MySqlDbType.Int32);
                    cmd.Parameters["@pProfileId"].Value = profileId;
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if(rdr.Read())
                        {
                            trainerProfile = new TrainerProfile();
                            trainerProfile.ProfileId = profileId;
                            trainerProfile.ReqId = Convert.ToInt32(rdr["ReqId"].ToString());
                            trainerProfile.TrainerName = rdr["TrainerName"].ToString();
                            trainerProfile.VendorId = rdr["VendorId"].ToString();
                            trainerProfile.SMEId = rdr["SMEId"].ToString();
                            trainerProfile.Feedback = rdr["Feedback"].ToString();
                            string rating = rdr["Rating"].ToString();
                            if (!rating.Equals(""))
                            {
                                trainerProfile.Rating = Convert.ToInt32(rdr["Rating"].ToString());
                            }
                            else
                            {
                                trainerProfile.Rating = 0;
                            }

                            
                        }
                    }
                }
            }

            return trainerProfile;
        }


        public static List<string> GetSMEs()
        {
            string SME;
            List<string> SMEs = new List<string>();
            using (MySqlConnection con = new MySqlConnection(MvcApplication.conStr))
            {
                con.Open();

                using (MySqlCommand cmd = new MySqlCommand("GetSMEs", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            SME = rdr["UserId"].ToString();
                            SMEs.Add(SME);
                        }
                    }
                }
            }

            return SMEs;
        }

        public static int SetSME(TrainerProfile trainerProfile)
        {

            int statusCode = 0;
            using (MySqlConnection con = new MySqlConnection(MvcApplication.conStr))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand("SetSME", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.Add("@pProfileId", MySqlDbType.Int32);
                    cmd.Parameters["@pProfileId"].Value = trainerProfile.ProfileId;

                    cmd.Parameters.Add("@pSMEId", MySqlDbType.VarChar, 20);
                    cmd.Parameters["@pSMEId"].Value = trainerProfile.SMEId;

                    cmd.Parameters.Add("@pStatusCode", MySqlDbType.Int32);
                    cmd.Parameters["@pStatusCode"].Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();
                    statusCode = Convert.ToInt32(cmd.Parameters["@pStatusCode"].Value);
                }
            }
            return statusCode;
        }

        public static List<TrainerProfile> GetTrainerProfilesSME(string SMEId)
        {
            TrainerProfile trainerProfile;
            List<TrainerProfile> trainerProfiles = new List<TrainerProfile>();

            using (MySqlConnection con = new MySqlConnection(MvcApplication.conStr))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand("GetTrainerProfilesSME", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("@pSMEId", MySqlDbType.VarChar,20);
                    cmd.Parameters["@pSMEId"].Value = SMEId;
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            trainerProfile = new TrainerProfile();
                            trainerProfile.ProfileId = Convert.ToInt32(rdr["ProfileId"].ToString());
                            trainerProfile.ReqId = Convert.ToInt32(rdr["ReqId"].ToString());
                            trainerProfile.ReqDesc = rdr["ReqDesc"].ToString();
                            trainerProfile.TrainerName = rdr["TrainerName"].ToString();
                            trainerProfile.Feedback = rdr["Feedback"].ToString();
                            string rating = rdr["Rating"].ToString();
                            if (!rating.Equals(""))
                            {
                                trainerProfile.Rating = Convert.ToInt32(rdr["Rating"].ToString());
                            }
                            else
                            {
                                trainerProfile.Rating = 0;
                            }

                            trainerProfiles.Add(trainerProfile);
                        }
                    }
                }
            }

            return trainerProfiles;
        }

        public static int SetFeedback(TrainerProfile trainerProfile)
        {

            int statusCode = 0;
            using (MySqlConnection con = new MySqlConnection(MvcApplication.conStr))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand("SetFeedback", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.Add("@pProfileId", MySqlDbType.Int32);
                    cmd.Parameters["@pProfileId"].Value = trainerProfile.ProfileId;

                    cmd.Parameters.Add("@pFeedback", MySqlDbType.VarChar, 100);
                    cmd.Parameters["@pFeedback"].Value = trainerProfile.Feedback;

                    cmd.Parameters.Add("@pRating", MySqlDbType.Int32);
                    cmd.Parameters["@pRating"].Value = trainerProfile.Rating;

                    cmd.Parameters.Add("@pStatusCode", MySqlDbType.Int32);
                    cmd.Parameters["@pStatusCode"].Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();
                    statusCode = Convert.ToInt32(cmd.Parameters["@pStatusCode"].Value);
                }
            }
            return statusCode;
        }

    }
}