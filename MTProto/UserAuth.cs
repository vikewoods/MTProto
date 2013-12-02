using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MTProto
{
  class UserAuth
  {
    public string DefualtPhoneNumber;
    public int DefualtSmsType;
    public int AppId;
    public string ApiHash;
    public string DefualtLanguageCode;

    ErrorsController errors = new ErrorsController();

    // Получаем данные с заполненной формы
    public void GetUserDetails()
    {
      
    }
    // Отправляет на переданный номер телефона СМС с кодом подтверждения, необходимым для регистрации.
    public void AuthUserWithSMS(string phone_number, int sms_type, int api_id, string api_hash, string lang_code)
    { 
      errors.ErrorForAuthUserWithSMS(400, "TEST");
    }
    // Совершает голосовой вызов на переданный номер телефона, в котором робот продублирует голосом код подтверждения из СМС сообщения.
    public void AuthUserWithCall(string phone_number, string phone_code_hash)
    {

    }
    // Возвращает информацию о том, зарегистрирован ли в системе переданный номер телефона.
    public void AuthCheckPhoneForRegistration(string phone_number)
    {

    }
    // Регистрирует завалидированный номер телефона в системе.
    public void SignUpUser(string phone_number, string phone_code_hash, string phone_code, string first_name, string last_name)
    {

    }
    // Авторизует пользователя в системе по завалидированному номеру телефона.
    public void SignInUser(string phone_number, string phone_code_hash, string phone_code)
    {

    }
    // Авторизует пользователя в системе по ключу, перенесённому из родного дата-центра.
    public void ImportAuthorizationUser(int id, byte[] bytes)
    {
    }
    // Возвращает текущую конфигурацию, в т.ч. конфигурацию дата-центров.
    public void GetUserConfig()
    {
    }
    // Возвращает информацию о ближайшем к пользователю дата-центре.
    public void GetNearDatacenter()
    {
    }
  }
}
