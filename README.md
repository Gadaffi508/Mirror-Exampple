Unity Multiplayer Sesli Sohbet Projesi
Bu proje, Unity'de Mirror ve Odin kullanarak multiplayer bir oyun içerisinde sesli sohbet özelliğini entegre etmeyi göstermektedir. Oyun içerisinde oda oluşturulduğunda, oyuncular otomatik olarak sesli sohbete bağlanır ve birbirleriyle iletişime geçebilirler.

Özellikler
🎙️ Sesli Sohbet: Oda kurulduğu anda oyuncular arasında sesli iletişim başlar.
🔄 Otomatik Entegrasyon: Her oyuncu odaya katıldığında sesli sohbeti otomatik olarak başlatır.
📡 Mirror ile Senkronizasyon: Unity’nin Mirror kütüphanesi kullanılarak multiplayer bağlantı ve senkronizasyon sağlanır.
🔧 Odin SDK ile Ayarlar: Unity Asset Store’dan indirilen Odin SDK ile sesli sohbet sisteminin kurulumu ve yapılandırılması yapılmıştır.
Kurulum
Mirror ve Odin Kurulumu: Proje Unity'de Mirror ve Odin SDK kullanılarak geliştirilmiştir.

Mirror ve Odin'i Unity Asset Store’dan indirin ve projeye dahil edin.
Unity Ayarları: Projeyi Unity üzerinden açın ve Mirror ayarlarını yapılandırın. Odin ayarları için VoiceChatManager gibi gerekli bileşenleri oyun sahnesine ekleyin.

Kod Detayları: Projeye ait temel sesli sohbet kodlarını VoiceChatManager.cs dosyasında bulabilirsiniz. Oda kurulumunda otomatik olarak başlatılacak şekilde yapılandırılmıştır.
Sample Kod
```
using UnityEngine;
using Mirror;

public class VoiceChatManager : NetworkBehaviour
{
    void Start()
    {
        if (isLocalPlayer)
        {
            // Odin sesli sohbet başlatma
            InitializeVoiceChat();
        }
    }

    void InitializeVoiceChat()
    {
        // Odin yapılandırma ayarları ve başlatma
        Debug.Log("Sesli sohbet başlatılıyor...");
        // Odin kurulum ve başlatma kodlarınızı buraya ekleyin.
    }
}

```

Kullanım
Oda Kurulumu: Oyuncular oyuna girdiğinde oda oluşturulur ve sesli sohbet otomatik olarak başlar.
Sesli Sohbet: Oyun içerisinde iletişime geçmek için ekstra bir ayar gerekmez; her oyuncu otomatik olarak sesli sohbete bağlanır.
Katkıda Bulunma
Bu projeye katkıda bulunmak isteyenler için pull request’ler açıktır. Her türlü geliştirme önerisi ve katkı memnuniyetle karşılanır.

Lisans
Bu proje MIT lisansı ile lisanslanmıştır. Detaylar için LICENSE dosyasına göz atabilirsiniz.

İlgili Etiketler
#Unity #Mirror #Odin #GameDevelopment #Multiplayer #VoiceChat #Unity3D #CSharp #IndieDev
