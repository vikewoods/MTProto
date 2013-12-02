using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTProto_Class_Api
{
  class UserAccount
  {
    // Регистрирует устройство для последующей отправки на него PUSH-уведомлений.
    public void UserRegisterDevice(int token_type, string token, string device_model, string system_version, string app_version, bool app_sandbox, string lang_code)
    {
    }
    //Удаляет устройство по его token'у, перестает отправлять на него PUSH-уведомления.
    public void UserUnregisterDevice(int token_type, string token)
    {
    }
    // 
  }
}
