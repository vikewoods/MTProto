using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MTProto_Class_Api
{
  class ErrorsController
  {

    public int ErrorId;
    public string ErrorType;

    UserAuth UserAuthController = new UserAuth();
    DeveloperAuth developerAuthController = new DeveloperAuth();
    Datacenter datacenterController = new Datacenter();

    // Ошибки для авторизации пользователя
    // Функции на возрашение ошибок связанных с авторизацией
    public void ErrorForAuthUserWithSMS(int error, string error_type)
    {
      error = ErrorId;
      error_type = ErrorType;

      if ((ErrorId == 400) && (ErrorType == "PHONE_NUMBER_INVALID"))
      {
        Debug.WriteLine("Некорректный телефонный номер");
      }

      if ((ErrorId == 303) && (ErrorType == "PHONE_MIGRATE_X"))
      {
        Debug.WriteLine("Необходимо повторить запрос к дата-центру X");
      }

      if ((ErrorId == 303) && (ErrorType == "NETWORK_MIGRATE_X"))
      {
        Debug.WriteLine("Необходимо повторить запрос к дата-центру X");
      }

    }
    public void ErrorForAuthUserWithCall(int error, string error_type)
    {
      error = ErrorId;
      error_type = ErrorType;

      if ((ErrorId == 400) && (ErrorType == "PHONE_NUMBER_INVALID"))
      {
        Debug.WriteLine("Некорректный телефонный номер");
      }

      if ((ErrorId == 400) && (ErrorType == "PHONE_CODE_HASH_EMPTY"))
      {
        Debug.WriteLine("Не передан параметр phone_code_hash");
      }

      if ((ErrorId == 400) && (ErrorType == "PHONE_CODE_EXPIRED"))
      {
        Debug.WriteLine("Истек срок действия СМС");
      }
    }
    public void ErrorForAuthCheckPhoneForRegistration(int error, string error_type)
    {
      error = ErrorId;
      error_type = ErrorType;

      if ((ErrorId == 400) && (ErrorType == "PHONE_NUMBER_INVALID"))
      {
        Debug.WriteLine("Некорректный телефонный номер");
      }
    }
    public void ErrorForSignUpUser(int error, string error_type)
    {
      error = ErrorId;
      error_type = ErrorType;

      if ((ErrorId == 400) && (ErrorType == "PHONE_NUMBER_INVALID"))
      {
        Debug.WriteLine("Некорректный телефонный номер");
      }

      if ((ErrorId == 400) && (ErrorType == "PHONE_CODE_EMPTY"))
      {
        Debug.WriteLine("Отсутствует параметр phone_code из СМС");
      }

      if ((ErrorId == 400) && (ErrorType == "PHONE_CODE_EXPIRED"))
      {
        Debug.WriteLine("Истек срок действия СМС");
      }

      if ((ErrorId == 400) && (ErrorType == "PHONE_CODE_INVALID"))
      {
        Debug.WriteLine("Передан неправильный код из СМС");
      }

      if ((ErrorId == 400) && (ErrorType == "FIRSTNAME_INVALID"))
      {
        Debug.WriteLine("Некорректное имя");
      }

      if ((ErrorId == 400) && (ErrorType == "LASTNAME_INVALID"))
      {
        Debug.WriteLine("Некорректная фамилия");
      }
    }
    public void ErrorForSignInUser(int error, string error_type)
    {
      error = ErrorId;
      error_type = ErrorType;

      if ((ErrorId == 400) && (ErrorType == "PHONE_NUMBER_INVALID"))
      {
        Debug.WriteLine("Некорректный телефонный номер");
      }

      if ((ErrorId == 400) && (ErrorType == "PHONE_CODE_EMPTY"))
      {
        Debug.WriteLine("Отсутствует параметр phone_code из СМС");
      }

      if ((ErrorId == 400) && (ErrorType == "PHONE_CODE_EXPIRED"))
      {
        Debug.WriteLine("Истек срок действия СМС");
      }

      if ((ErrorId == 400) && (ErrorType == "PHONE_CODE_INVALID"))
      {
        Debug.WriteLine("Передан неправильный код из СМС");
      }

      if ((ErrorId == 400) && (ErrorType == "PHONE_NUMBER_UNOCCUPIED"))
      {
        Debug.WriteLine("Код верен, но пользователя с таким номером телефона не существует");
      }
    }
  }
}
