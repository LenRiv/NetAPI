using Net_API.Modelos.Dto;

namespace Net_API.Datos
{
    public static class VillaStore
    {
        public static List<VillaDto> villaList = new List<VillaDto>
        {
            new VillaDto{Id=1, Nombre="Vista a la piscina", Inquilinos=3, MetrosCuadrados=50},
            new VillaDto{Id=2, Nombre="Vista a la playa", Inquilinos=4, MetrosCuadrados=80}

        };
    }
}
