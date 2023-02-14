using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Net_API.Datos;
using Net_API.Modelos;
using Net_API.Modelos.Dto;

namespace Net_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ILogger<VillaController> _logger;//Lo creamos nosotros
        private readonly AppDbContext _db;//Creamos esta propiedad para inyeccion DbContext
            

        public VillaController(ILogger<VillaController> logger, AppDbContext db)
        {

            _logger = logger;
            _db = db;
        
        }








        [HttpGet]//Devuelve toda la lista
        [ProducesResponseType(StatusCodes.Status200OK)]//endpoint Documentado

        public ActionResult<IEnumerable<VillaDto>> GetVillas()
        {
            _logger.LogInformation("Obtener todas las casas");
            //return Ok(VillaStore.villaList);
            return Ok(_db.Villas.ToList());//Es como select * from de la tabla Villa
        }



        [HttpGet("{id:int}", Name ="GetVilla")]//Devuelve un solo objeto segun su ID y el nuevo creado
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<VillaDto> GetVilla(int id)
        {
            if (id == 0)
            {
                _logger.LogError("Error al traer Casa con Id " + id); 
                return BadRequest(); //400
            }
            //El modelo que usamos despues del ActionResult
            //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            var villa = _db.Villas.FirstOrDefault(v => v.Id == id);//Implementamos la base de datos

            if (villa == null)
            {
                return NotFound();//404
            }

            return Ok(villa);//200
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]//Error interno
        public ActionResult<VillaDto>CrearVilla([FromBody] VillaDto villaDto)
        { 

            if (!ModelState.IsValid)//Validar el Modelo, no dejar sin nombre, sin Id....
            {
                return BadRequest(ModelState);
            }
        
            //Validacion personalizada para comparar nombres
            if (_db.Villas.FirstOrDefault(v=>v.Nombre.ToLower() == villaDto.Nombre.ToLower()) !=null)
            {
                ModelState.AddModelError("NombreExiste", "Ese registro ya existe!");
                return BadRequest(ModelState);//Retorna el ModelState personalizado que he creado
            }
          if(villaDto == null) 
            {
                return BadRequest(villaDto);//Porque no nos envian datos(null)
            }
        
            if(villaDto.Id>0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            //Capturamos el Id del siguiente registro
            //villaDto.Id = VillaStore.villaList.OrderByDescending(v=>v.Id).FirstOrDefault().Id + 1;
            //Agregamos el nuevo objeto a la lista
            //VillaStore.villaList.Add(villaDto);

            //return Ok(villaDto); //Retornamos el nuevo registro de prueba
            //Lo correcto es hacer el retorno del Get de un solo elemento

            //Creamos un nuevo modelo en base a la BD
            Villa modelo = new()
            {                               
                Nombre = villaDto.Nombre,   //Id = villaDto.Id, //Se crea automatico
                Detalle = villaDto.Detalle,
                Tarifa = villaDto.Tarifa,
                Inquilinos = villaDto.Inquilinos,
                MetrosCuadrados = villaDto.MetrosCuadrados,
                ImagenUrl = villaDto.ImagenUrl,
                Encanto = villaDto.Encanto

            };

            _db.Villas.Add(modelo);//insert
            _db.SaveChanges();//Para que los cambios se vean reflejados en la base de datos enviando el 
            //nuevo modelo creado

            return CreatedAtRoute("GetVilla", new {id= villaDto.Id, villaDto});
            //Creamos ruta a Get ID y devolvemos todo el modelo desde el Id de villaDto
        

        }


        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteVilla(int id) 
        {
        
           if(id==0) 
            {
            return  BadRequest();//Id sin valor
            }
                        //VillaStore.villaList
            var villa = _db.Villas.FirstOrDefault(v=>v.Id == id);
            if(villa==null) //Si no encuentra una villa en la variable anterior....
            { 
              return NotFound();
            }
            //VillaStore.villaList.Remove(villa);//Si pasa los filtros, lo eliminamos

            _db.Villas.Remove(villa);//Para hacer delete en BD
            _db.SaveChanges();


            return NoContent();//Retornamos vacio
        
        }


        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateVilla(int id, [FromBody] VillaDto villaDto) 
        {
        
         if(villaDto==null || id!= villaDto.Id)
            {
                return BadRequest();//Si no coinciden los ID para la actualizacion
            }
            //Para buscar el registro que quireo modificar
            //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            //villa.Nombre = villaDto.Nombre;
            //villa.Inquilinos = villaDto.Inquilinos;
            //villa.MetrosCuadrados = villaDto.MetrosCuadrados;

            //Creamos modelo tipo villa
            Villa modelo = new()
            {
                Id = villaDto.Id,
                Nombre = villaDto.Nombre,
                Detalle = villaDto.Detalle,
                Tarifa = villaDto.Tarifa,
                Inquilinos = villaDto.Inquilinos,
                MetrosCuadrados = villaDto.MetrosCuadrados,
                ImagenUrl = villaDto.ImagenUrl,
                Encanto = villaDto.Encanto

            };

            _db.Villas.Update(modelo);
            _db.SaveChanges();

            return NoContent();//No retorna ningún modelo

        }



        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDto> patchDto)
        {

            if (patchDto == null || id == 0)
            {
                return BadRequest();//Si no coinciden los ID para la actualizacion
            }
            //Para buscar el registro que quireo modificar
           // var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);

            var villa = _db.Villas.AsNoTracking().FirstOrDefault(v => v.Id == id);//Capturar el registro que se va a modificar

            VillaDto villaDto = new()
            {
                //Ponemos los datos antes de los cambios
                Id = villa.Id,
                Nombre = villa.Nombre,
                Detalle = villa.Detalle,
                Tarifa = villa.Tarifa,
                Inquilinos = villa.Inquilinos,
                MetrosCuadrados = villa.MetrosCuadrados,
                ImagenUrl = villa.ImagenUrl,
                Encanto = villa.Encanto
             };

            if(villa == null) return BadRequest();

            patchDto.ApplyTo(villaDto, ModelState);

            Villa modelo = new Villa()
            {
                //Los dato despues de los cambios, esto se lo enviamos al Db que esta en nuestro Update
                Id = villa.Id,
                Nombre = villaDto.Nombre,
                Detalle = villaDto.Detalle,
                Tarifa = villaDto.Tarifa,
                Inquilinos = villaDto.Inquilinos,
                MetrosCuadrados = villaDto.MetrosCuadrados,
                ImagenUrl = villaDto.ImagenUrl,
                Encanto = villaDto.Encanto
             };

            _db.Villas.Update(modelo);
            _db.SaveChanges();

            return NoContent();//No retorna ningún modelo

        }



    }
}
