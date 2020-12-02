namespace BasicApi.Items.Exceptions
{
    public class ExceptionMessages
    {
        public static string ImageNotAvaible => "resim formatına uygun değildir.";

        public static string DocumentNotAvaible => "pdf formatı değildir.";

        public static string UserAlreadyExists => "Kullanıcı adı daha önce kullanılmaktadır.";

        public static string UsernameAndPasswordError => "Kullanıcı adı veya şifre hatalı!";

        public static string UserNotFound => "Kullanıcı Bulunamadı.";

        public static string RoleNotFound => "Rol Bulunamadı.";

        public static string MailNotAvaible => "Geçersiz mail adresi.";

    }
}
