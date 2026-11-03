using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace Atividade_3.Models;

public partial class BdAulaBackendContext : DbContext
{
    public BdAulaBackendContext()
    {
    }

    public BdAulaBackendContext(DbContextOptions<BdAulaBackendContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Atendimento> Atendimentos { get; set; }

    public virtual DbSet<AtendimentoCompleto> AtendimentoCompletos { get; set; }

    public virtual DbSet<Atendimentoitem> Atendimentoitems { get; set; }

    public virtual DbSet<Categorium> Categoria { get; set; }

    public virtual DbSet<Endereco> Enderecos { get; set; }

    public virtual DbSet<EntradaProduto> EntradaProdutos { get; set; }

    public virtual DbSet<Estoque> Estoques { get; set; }

    public virtual DbSet<Funcionario> Funcionarios { get; set; }

    public virtual DbSet<FuncionarioEndereco> FuncionarioEnderecos { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<ItemCategoriaEstoque> ItemCategoriaEstoques { get; set; }

    public virtual DbSet<ItemCategorium> ItemCategoria { get; set; }

    public virtual DbSet<Pessoa> Pessoas { get; set; }

    public virtual DbSet<PessoaEndereco> PessoaEnderecos { get; set; }

    public virtual DbSet<Produto> Produtos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb3_general_ci")
            .HasCharSet("utf8mb3");

        modelBuilder.Entity<Atendimento>(entity =>
        {
            entity.HasKey(e => e.Idatendimento).HasName("PRIMARY");

            entity.ToTable("atendimento");

            entity.HasIndex(e => e.Idpessoa, "fk_atendimento_pessoa1_idx");

            entity.Property(e => e.Idatendimento).HasColumnName("idatendimento");
            entity.Property(e => e.DataAtedimento).HasColumnName("data_atedimento");
            entity.Property(e => e.Idpessoa).HasColumnName("idpessoa");
            entity.Property(e => e.StatusAtedimento)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasColumnName("status_atedimento")
                .UseCollation("utf8mb3_bin");

            entity.HasOne(d => d.IdpessoaNavigation).WithMany(p => p.Atendimentos)
                .HasForeignKey(d => d.Idpessoa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_atendimento_pessoa1");
        });

        modelBuilder.Entity<AtendimentoCompleto>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("atendimento_completo");

            entity.Property(e => e.Cliente)
                .HasMaxLength(100)
                .HasColumnName("cliente");
            entity.Property(e => e.DataAtedimento).HasColumnName("data_atedimento");
            entity.Property(e => e.Funcionario)
                .HasMaxLength(20)
                .HasColumnName("funcionario");
            entity.Property(e => e.Idatendimento).HasColumnName("idatendimento");
            entity.Property(e => e.Idfuncionario).HasColumnName("idfuncionario");
            entity.Property(e => e.Produto)
                .HasMaxLength(40)
                .HasColumnName("produto");
            entity.Property(e => e.QuantidadeItens).HasColumnName("quantidade_itens");
            entity.Property(e => e.ValorEntrada)
                .HasColumnType("decimal(10,2) unsigned")
                .HasColumnName("valor_entrada");
        });

        modelBuilder.Entity<Atendimentoitem>(entity =>
        {
            entity.HasKey(e => new { e.Iditem, e.Idatendimento })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("atendimentoitem");

            entity.HasIndex(e => e.Idfuncionario, "fk_atendimentoitem_funcionario1_idx");

            entity.HasIndex(e => e.Idatendimento, "fk_item_has_atendimento_atendimento1_idx");

            entity.HasIndex(e => e.Iditem, "fk_item_has_atendimento_item_idx");

            entity.Property(e => e.Iditem).HasColumnName("iditem");
            entity.Property(e => e.Idatendimento).HasColumnName("idatendimento");
            entity.Property(e => e.Idfuncionario).HasColumnName("idfuncionario");
            entity.Property(e => e.QuantidadeItens).HasColumnName("quantidade_itens");
            entity.Property(e => e.ValorunitarioEntrada)
                .HasColumnType("decimal(10,2) unsigned")
                .HasColumnName("valorunitario_entrada");

            entity.HasOne(d => d.IdatendimentoNavigation).WithMany(p => p.Atendimentoitems)
                .HasForeignKey(d => d.Idatendimento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_item_has_atendimento_atendimento1");

            entity.HasOne(d => d.IdfuncionarioNavigation).WithMany(p => p.Atendimentoitems)
                .HasForeignKey(d => d.Idfuncionario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_atendimentoitem_funcionario1");

            entity.HasOne(d => d.IditemNavigation).WithMany(p => p.Atendimentoitems)
                .HasForeignKey(d => d.Iditem)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_item_has_atendimento_item");
        });

        modelBuilder.Entity<Categorium>(entity =>
        {
            entity.HasKey(e => e.Idcategoria).HasName("PRIMARY");

            entity.ToTable("categoria");

            entity.Property(e => e.Idcategoria).HasColumnName("idcategoria");
            entity.Property(e => e.Categoria)
                .HasMaxLength(35)
                .HasColumnName("categoria");
        });

        modelBuilder.Entity<Endereco>(entity =>
        {
            entity.HasKey(e => e.Idendereco).HasName("PRIMARY");

            entity.ToTable("endereco");

            entity.HasIndex(e => e.Idpessoa, "fk_endereco_pessoa1_idx");

            entity.Property(e => e.Idendereco).HasColumnName("idendereco");
            entity.Property(e => e.Cep)
                .HasMaxLength(8)
                .IsFixedLength()
                .HasColumnName("cep");
            entity.Property(e => e.Complemento)
                .HasMaxLength(150)
                .HasColumnName("complemento");
            entity.Property(e => e.Idpessoa).HasColumnName("idpessoa");
            entity.Property(e => e.Numero)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("numero");

            entity.HasOne(d => d.IdpessoaNavigation).WithMany(p => p.Enderecos)
                .HasForeignKey(d => d.Idpessoa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_endereco_pessoa1");
        });

        modelBuilder.Entity<EntradaProduto>(entity =>
        {
            entity.HasKey(e => new { e.IdentradaProduto, e.Iditem })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("entrada_produto");

            entity.HasIndex(e => e.Iditem, "fk_entrada_produto_estoque1_idx");

            entity.Property(e => e.IdentradaProduto)
                .ValueGeneratedOnAdd()
                .HasColumnName("identrada_produto");
            entity.Property(e => e.Iditem).HasColumnName("iditem");
            entity.Property(e => e.DataEntradaProduto).HasColumnName("data_entrada_produto");
            entity.Property(e => e.QuantidadeEntrada)
                .HasColumnType("decimal(10,2) unsigned")
                .HasColumnName("quantidade_entrada");
            entity.Property(e => e.Unidade)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("unidade");
            entity.Property(e => e.ValorUnitarioEntrada)
                .HasColumnType("decimal(10,2) unsigned")
                .HasColumnName("valor_unitario_entrada");

            entity.HasOne(d => d.IditemNavigation).WithMany(p => p.EntradaProdutos)
                .HasForeignKey(d => d.Iditem)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_entrada_produto_estoque1");
        });

        modelBuilder.Entity<Estoque>(entity =>
        {
            entity.HasKey(e => e.Iditem).HasName("PRIMARY");

            entity.ToTable("estoque");

            entity.Property(e => e.Iditem)
                .ValueGeneratedNever()
                .HasColumnName("iditem");
            entity.Property(e => e.QuantidadeEstoque).HasColumnName("quantidade_estoque");

            entity.HasOne(d => d.IditemNavigation).WithOne(p => p.Estoque)
                .HasForeignKey<Estoque>(d => d.Iditem)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_estoque_produto1");
        });

        modelBuilder.Entity<Funcionario>(entity =>
        {
            entity.HasKey(e => e.Idpessoa).HasName("PRIMARY");

            entity.ToTable("funcionario");

            entity.Property(e => e.Idpessoa)
                .ValueGeneratedNever()
                .HasColumnName("idpessoa");
            entity.Property(e => e.Cargo)
                .HasMaxLength(20)
                .HasColumnName("cargo");
            entity.Property(e => e.DataAdmissao).HasColumnName("data_admissao");

            entity.HasOne(d => d.IdpessoaNavigation).WithOne(p => p.Funcionario)
                .HasForeignKey<Funcionario>(d => d.Idpessoa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_funcionario_pessoa1");
        });

        modelBuilder.Entity<FuncionarioEndereco>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("funcionario_endereco");

            entity.Property(e => e.Cargo)
                .HasMaxLength(20)
                .HasColumnName("cargo");
            entity.Property(e => e.DataAdmissao).HasColumnName("data_admissao");
            entity.Property(e => e.Idpessoa).HasColumnName("idpessoa");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.Iditem).HasName("PRIMARY");

            entity.ToTable("item");

            entity.HasIndex(e => e.CategoriaIdcategoria, "fk_item_categoria1_idx");

            entity.Property(e => e.Iditem).HasColumnName("iditem");
            entity.Property(e => e.AtivoProdutoServico)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasColumnName("ativo_produto_servico");
            entity.Property(e => e.CategoriaIdcategoria).HasColumnName("categoria_idcategoria");
            entity.Property(e => e.DescricaoItem)
                .HasMaxLength(200)
                .HasColumnName("descricao_item");
            entity.Property(e => e.Item1)
                .HasMaxLength(40)
                .HasColumnName("item");
            entity.Property(e => e.Preco)
                .HasColumnType("decimal(10,2) unsigned")
                .HasColumnName("preco");
            entity.Property(e => e.TipoItem)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasColumnName("tipo_item");

            entity.HasOne(d => d.CategoriaIdcategoriaNavigation).WithMany(p => p.Items)
                .HasForeignKey(d => d.CategoriaIdcategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_item_categoria1");
        });

        modelBuilder.Entity<ItemCategoriaEstoque>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("item_categoria_estoque");

            entity.Property(e => e.Categoria)
                .HasMaxLength(35)
                .HasColumnName("categoria");
            entity.Property(e => e.Item)
                .HasMaxLength(40)
                .HasColumnName("item");
            entity.Property(e => e.QuantidadeEstoque).HasColumnName("quantidade_estoque");
        });

        modelBuilder.Entity<ItemCategorium>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("item_categoria");

            entity.Property(e => e.AtivoProdutoServico)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasColumnName("ativo_produto_servico");
            entity.Property(e => e.Categoria)
                .HasMaxLength(35)
                .HasColumnName("categoria");
            entity.Property(e => e.DescricaoItem)
                .HasMaxLength(200)
                .HasColumnName("descricao_item");
            entity.Property(e => e.Idcategoria).HasColumnName("idcategoria");
            entity.Property(e => e.Iditem).HasColumnName("iditem");
            entity.Property(e => e.Item)
                .HasMaxLength(40)
                .HasColumnName("item");
            entity.Property(e => e.Preco)
                .HasColumnType("decimal(10,2) unsigned")
                .HasColumnName("preco");
            entity.Property(e => e.TipoItem)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasColumnName("tipo_item");
        });

        modelBuilder.Entity<Pessoa>(entity =>
        {
            entity.HasKey(e => e.Idpessoa).HasName("PRIMARY");

            entity.ToTable("pessoa");

            entity.HasIndex(e => e.Cpf, "cpf_UNIQUE").IsUnique();

            entity.HasIndex(e => e.Email, "email_UNIQUE").IsUnique();

            entity.Property(e => e.Idpessoa).HasColumnName("idpessoa");
            entity.Property(e => e.AtivoPessoa)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasColumnName("ativo_pessoa")
                .UseCollation("utf8mb3_bin");
            entity.Property(e => e.Cpf).HasColumnName("cpf");
            entity.Property(e => e.DataNascimento).HasColumnName("data_nascimento");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .HasColumnName("nome");
            entity.Property(e => e.Sexo)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasColumnName("sexo");
            entity.Property(e => e.Telefone).HasColumnName("telefone");
        });

        modelBuilder.Entity<PessoaEndereco>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("pessoa_endereco");

            entity.Property(e => e.Cep)
                .HasMaxLength(8)
                .IsFixedLength()
                .HasColumnName("cep");
            entity.Property(e => e.Complemento)
                .HasMaxLength(150)
                .HasColumnName("complemento");
            entity.Property(e => e.Cpf).HasColumnName("cpf");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .HasColumnName("nome");
            entity.Property(e => e.Numero)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("numero");
        });

        modelBuilder.Entity<Produto>(entity =>
        {
            entity.HasKey(e => e.Iditem).HasName("PRIMARY");

            entity.ToTable("produto");

            entity.Property(e => e.Iditem)
                .ValueGeneratedNever()
                .HasColumnName("iditem");
            entity.Property(e => e.CodigoPreco).HasColumnName("codigo_preco");
            entity.Property(e => e.PrecoVenda)
                .HasColumnType("decimal(10,2) unsigned")
                .HasColumnName("preco_venda");
            entity.Property(e => e.Unidade)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("unidade");

            entity.HasOne(d => d.IditemNavigation).WithOne(p => p.Produto)
                .HasForeignKey<Produto>(d => d.Iditem)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_produto_item1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
