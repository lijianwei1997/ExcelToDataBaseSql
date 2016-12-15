using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 连接数据库
{
    class User
    {

        private string _Ip;
        private string _User;


        private string _Password;

        public string Password
        {
            get
            {
                return _Password;
            }

            set
            {
                _Password = value;
            }
        }

    

        public string Ip
        {
            get
            {
                return _Ip;
            }

            set
            {
                _Ip = value;
            }
        }

        public string USer
        {
            get
            {
                return _User;
            }

            set
            {
                _User = value;
            }
        }
    }
}
