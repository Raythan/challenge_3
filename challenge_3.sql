-- phpMyAdmin SQL Dump
-- version 5.0.2
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Tempo de geração: 02-Ago-2020 às 20:18
-- Versão do servidor: 10.4.13-MariaDB
-- versão do PHP: 7.4.7

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Banco de dados: `challenge_3`
--

-- --------------------------------------------------------

--
-- Estrutura da tabela `cadastro`
--

CREATE TABLE `cadastro` (
  `ID` bigint(20) NOT NULL COMMENT 'Coluna Identidade da Tabela',
  `NOME` varchar(50) NOT NULL COMMENT 'Nome da pessoa',
  `GENERO` varchar(255) NOT NULL COMMENT 'Gênero que a pessoa se identifica',
  `EMAIL` varchar(100) DEFAULT NULL COMMENT 'Email para contato',
  `DATANASCIMENTO` date NOT NULL COMMENT 'Data de nascimento da pessoa',
  `NATURALIDADE` varchar(30) NOT NULL COMMENT 'Estado de nascimento da pessoa',
  `NACIONALIDADE` varchar(30) NOT NULL COMMENT 'País de nascimento da pessoa',
  `CPF` varchar(14) NOT NULL COMMENT 'Número de documento da pessoa',
  `DATACADASTRO` datetime NOT NULL DEFAULT current_timestamp() COMMENT 'Data de cadastro da pessoa',
  `DATAATUALIZACAO` datetime NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp() COMMENT 'Data de controle de atualizações da pessoa'
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Tabela que armazena os cadastros de pessoa';

--
-- Extraindo dados da tabela `cadastro`
--

INSERT INTO `cadastro` (`ID`, `NOME`, `GENERO`, `EMAIL`, `DATANASCIMENTO`, `NATURALIDADE`, `NACIONALIDADE`, `CPF`, `DATACADASTRO`, `DATAATUALIZACAO`) VALUES
(7, 'Raythan Padovani Abreu', 'Masculino', 'a@a.com', '2003-08-14', 'TESTE', 'TESTE', '144.651.857-40', '2020-07-23 23:26:26', '2020-07-30 22:22:12');

-- --------------------------------------------------------

--
-- Estrutura da tabela `header`
--

CREATE TABLE `header` (
  `id` int(11) NOT NULL,
  `login` varchar(255) NOT NULL,
  `password` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Extraindo dados da tabela `header`
--

INSERT INTO `header` (`id`, `login`, `password`) VALUES
(1, 'raythan', 'raythan100');

--
-- Índices para tabelas despejadas
--

--
-- Índices para tabela `cadastro`
--
ALTER TABLE `cadastro`
  ADD PRIMARY KEY (`ID`);

--
-- Índices para tabela `header`
--
ALTER TABLE `header`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT de tabelas despejadas
--

--
-- AUTO_INCREMENT de tabela `cadastro`
--
ALTER TABLE `cadastro`
  MODIFY `ID` bigint(20) NOT NULL AUTO_INCREMENT COMMENT 'Coluna Identidade da Tabela', AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT de tabela `header`
--
ALTER TABLE `header`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
