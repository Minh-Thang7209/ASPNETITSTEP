namespace ASPNETITSTEP.Services.Kdf
{
    // KDF key derivation function By RFC 2898
    public interface IKdfService
    {
        String Dk(String password, String salt );
        
    }
}