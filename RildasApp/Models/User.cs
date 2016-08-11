using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RildasApp.Models
{
    public class User
    {
        public static UserState ParseState(string state)
        {
            switch (state)
            {
                case "OK": return UserState.OK;
                case "BANNED": return UserState.BANNED;
                case "PENDING": return UserState.PENDING;
                case "DELETED": return UserState.DELETED;
            }
            return UserState.OK;
        }
        public int id;
        public string username;
        public string password;
        public int access;
        public string email;
        public UserState userState;
        // We must erase hash for enabled login
        public string hash;
        public DateTime registrationTime;
        public string registrationIP;
        public DateTime lastLoginTime;
        public string lastLoginIP;
        public string passwordHash;
        public Access[] accessList;
        public enum UserState { OK, BANNED, PENDING, DELETED }

        public enum Access
        {
            ADMIN_ACCESS,
            ADMIN_ADDANIME,
            ADMIN_CHANGEANIME,
            ADMIN_NOTIFICATIONS,
            ADMIN_PUBLISHAPP,
            ADMIN_USEREDITGENERAL,
            ADMIN_TEAMEDITGENERAL,
            ADMIN_TEAMEDITTRANSLATE,
            ADMIN_TEAMEDITCORRECTION,
            ADMIN_TEAMEDITAPPROVAL,
            ADMIN_TEAMEDITPUBLISH,
            ADMIN_TEAMEDITENCODE,
            ADMIN_ADDADMIN,
            ADMIN_REMOVEADMIN,
            ADMIN_PUBLISHSERVER,
            ADMIN_RESTARTSERVER,
            APP_ACCESS,
            APPROVE_ANIME,
            APPROVAL_SKIP,
            CHAT_CREATENEW,
            CORRECTION_FOREIGN,
            CORRECTION_RESERVEFOREIGN,
            CORRECTION_UPLOAD,
            CORRECTION_UPLOADRESERVED,
            ENCODE_ANIME,
            PUBLISH_OWN,
            PUBLISH_FOREIGN,
            TRANSLATE_OWN,
            TRANSLATE_FOREIGN
        };
    }
}
