using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProyectoVirtualStore.Data;
using ProyectoVirtualStore.Helpers;
using ProyectoVirtualStore.Models;
using System.Data;
using System.Diagnostics.Metrics;

#region

//ALTER VIEW V_COMENTARIO_USUARIO
//AS
//	SELECT c.id_comentario, u.id_usuario, u.nombre_usuario , u.imagen, c.id_juego , c.comentario , c.fecha_post
//	FROM USUARIOS u
//	INNER JOIN COMENTARIOS c ON u.id_usuario=c.id_usuario
//	ORDER BY c.fecha_post
//GO






//ALTER PROCEDURE SP_GRUPO_JUEGOS_FILTROS
//(@posicion INT, @categoria NVARCHAR(50),@precio DECIMAL
//, @numeroregistros INT OUT)
//AS
//    declare @idcategoria int
//	select @idcategoria = c.id_categoria
//	from categorias c
//	where c.nombre_categoria=@categoria

//	SELECT @numeroregistros = COUNT(jc.id_juego) 
//	FROM juegos_categorias jc 
//	INNER JOIN juegos j
//	ON j.id_juego = jc.id_juego
//	WHERE jc.id_categoria = @idcategoria and j.precio_juego <= @precio;

//SELECT id_juego, nombre_juego, descripcion_juego, precio_juego, estado, nombreimagen FROM
//        (SELECT CAST(
//            ROW_NUMBER() OVER(ORDER BY j.nombre_juego) AS INT) AS POSICION,
//           j.id_juego, j.nombre_juego , j.descripcion_juego, j.precio_juego, j.estado, j.nombreimagen
//        FROM JUEGOS j
//		INNER JOIN juegos_categorias jc ON j.id_juego = jc.id_juego
//        WHERE jc.id_categoria=@idcategoria and j.precio_juego <= @precio) AS QUERY
//    WHERE QUERY.POSICION >= @POSICION AND QUERY.POSICION < (@POSICION + 4)

//GO
#endregion





namespace ProyectoVirtualStore.Repository
{
    public class RepositorySQLTienda : IRepository
    {
        private TiendaContext context;

        public RepositorySQLTienda(TiendaContext context)
        {
            this.context = context;
        }

        public async Task<List<Juegos>> GetJuegos()
        {
            var consulta = from datos in this.context.Juegos
                           select datos;
            return await consulta.ToListAsync();
        }

        public async Task<List<Juegos>> GetJuegosEstados(string estado)
        {
            var consulta = from datos in this.context.Juegos
                           where datos.Estado == estado
                           select datos;
            return await consulta.ToListAsync();
        }

        public async Task<Juegos> GetJuego(int id)
        {
            return await this.context.Juegos.FirstOrDefaultAsync(x => x.IdJuego == id);
        }
        private int GetMaxIdUsuario()
        {
            if (this.context.Usuarios.Count() == 0)
            {
                return 1;
            }
            else
            {
                return this.context.Usuarios.Max(z => z.IdUsuario) + 1;
            }

        }

        public async Task RegisterUser(string nombreusuario, string password, string email)
        {
            Usuario usuario = new Usuario();
            usuario.IdUsuario = this.GetMaxIdUsuario();
            usuario.NombreUsuario = nombreusuario;
            usuario.Email = email;
            usuario.Salt = HelperCryptography.GenerateSalt();
            usuario.Password = HelperCryptography.EncryptPassword(password, usuario.Salt);

            this.context.Usuarios.Add(usuario);
            await this.context.SaveChangesAsync();
        }

        public async Task<Usuario> LogInUser(string email, string password)
        {
            List<Usuario> lista = await this.context.Usuarios.ToListAsync();
            Usuario user = this.context.Usuarios.FirstOrDefault(x => x.Email == email);
            if (user == null)
            {
                return null;
            }
            else
            {

                byte[] passUsuario = user.Password;

                string salt = user.Salt;
                byte[] temp =
                    HelperCryptography.EncryptPassword(password, salt);

                bool respuesta =
                    HelperCryptography.CompareArrays(passUsuario, temp);
                if (respuesta == true)
                {
                    //SON IGUALES
                    return user;
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<List<Comentarios>> GetComentarios(int idjuego)
        {
            return await this.context.Comentarios.Where(x => x.IdJuego == idjuego).ToListAsync();
        }


        public async Task<List<VistaComentarios>> GetVistaComentarios(int id)
        {

            return await this.context.VistaComentarios.Where(x => x.IdJuego == id).ToListAsync();


        }

        private int MaxIdComentario()
        {

            if (this.context.Comentarios.Count() == 0)
            {
                return 1;
            }
            else
            {
                return this.context.Comentarios.Max(z => z.IdComentario) + 1;
            }

        }

        public async Task InsertComentarios(int idjuego, int idusuario, string comentario, DateTime fecha)
        {
            Comentarios comentarios = new Comentarios();
            comentarios.Comentario = comentario;
            comentarios.IdUsuario = idusuario;
            comentarios.IdJuego = idjuego;
            comentarios.IdComentario = this.MaxIdComentario();
            comentarios.FechaPost = fecha;

            this.context.Comentarios.Add(comentarios);
            //GUARDAMOS CAMBIOS EN LA BASE DE DATOS
            await this.context.SaveChangesAsync();
        }

        public async Task<List<Categorias>> GetCategorias()
        {
            return await this.context.Categorias.ToListAsync();
        }

        public async Task<ModelPaginarJuegos> GetJuegosFiltros(int posicion, Decimal precio, string categoria)
        {
            string sql = "SP_GRUPO_JUEGOS_FILTROS @posicion, @categoria , @precio , @numeroregistros OUT";
            SqlParameter pamposicion =
                new SqlParameter("@posicion", posicion);
            SqlParameter pamcategoria =
                new SqlParameter("@categoria", categoria);
            SqlParameter pamprecio =
               new SqlParameter("@precio", precio);
            SqlParameter pamnumerosregistros =
               new SqlParameter("@numeroregistros", -1);

            pamnumerosregistros.Direction = ParameterDirection.Output;
            var consulta =
                this.context.Juegos.FromSqlRaw(sql, pamposicion, pamcategoria, pamprecio, pamnumerosregistros);
            List<Juegos> juegos = await consulta.ToListAsync();
            int registros = (int)pamnumerosregistros.Value;
            return new ModelPaginarJuegos
            {
                NumeroRegistros = registros,
                Juegos = juegos
            };


        }


        public async Task<List<Juegos>> GetJuegosCarritosAsync(List<int> idjuegos) {

            return await this.context.Juegos.Where(x => idjuegos.Contains(x.IdJuego)).ToListAsync();
        }



        private int GetMaxIdCompra()
        {
            if (this.context.Compras.Count() == 0)
            {
                return 1;
            }
            else
            {
                return this.context.Compras.Max(z => z.IdCompra) + 1;
            }

        }


        public async Task InsertarCompra(List<Juegos> juegos , int idUsuario,DateTime fecha)
        {
            Decimal preciototal = 0;

            int idcompra = this.GetMaxIdCompra();
            Compra compra = new Compra();
            compra.IdCompra = idcompra;
            compra.IdUsuario = idUsuario;
            compra.PrecioTotal = 0;
            compra.FechaCompra = fecha;

              await this.context.Compras.AddAsync(compra);
             await  this.context.SaveChangesAsync();

            for (int i = 0; i < juegos.Count; i++) {
                preciototal += juegos[i].PrecioJuego;
                CompraJuego compraJuego = new CompraJuego();
                compraJuego.IdCompra = idcompra;
                compraJuego.IdJuego = juegos[i].IdJuego;
                compraJuego.PrecioJuego = juegos[i].PrecioJuego;
                this.context.Add(compraJuego);
               
            }
            await this.context.SaveChangesAsync();


            Compra compraFinal = await FindCompra(idcompra);

            compraFinal.PrecioTotal= preciototal;
            this.context.Attach(compraFinal);
            await this.context.SaveChangesAsync();

        }


        public async Task<Compra> FindCompra(int idcompra) {
            return await this.context.Compras.FirstOrDefaultAsync(x => x.IdCompra==idcompra);
        }

        public async Task<Usuario> FindUsuario(int idususario) 
        {
           return await this.context.Usuarios.FirstOrDefaultAsync(x => x.IdUsuario == idususario);
        }

        public async Task ModificarUsuarioImagen(int idususario , string imagen) 
        {
            Usuario usuario = await FindUsuario(idususario);
            usuario.Imagen = imagen;
            this.context.Attach(usuario);
            await this.context.SaveChangesAsync();
        }

        public async  Task<List<Imagenes>> GetImagenes(int idjuego)
        {
            return await this.context.Imagenes.Where(x => x.IdJuego == idjuego).ToListAsync();
        }

       
    }
}
