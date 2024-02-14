-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Feb 14, 2024 at 01:43 PM
-- Server version: 10.4.27-MariaDB
-- PHP Version: 8.2.0

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `mitybosdb`
--

-- --------------------------------------------------------

--
-- Table structure for table `maisto_produktas`
--

CREATE TABLE `maisto_produktas` (
  `id_produktas` int(6) NOT NULL,
  `produkto_pavadinimas` varchar(50) NOT NULL,
  `kcal` decimal(7,2) UNSIGNED NOT NULL DEFAULT 0.00,
  `angliavandeniai` decimal(7,2) UNSIGNED NOT NULL DEFAULT 0.00,
  `baltymai` decimal(7,2) UNSIGNED NOT NULL DEFAULT 0.00,
  `riebalai` decimal(7,2) UNSIGNED NOT NULL DEFAULT 0.00
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `naudotojas`
--

CREATE TABLE `naudotojas` (
  `id_naudotojas` int(6) NOT NULL,
  `naudotojo_vardas` varchar(50) NOT NULL,
  `slaptazodis` varchar(255) NOT NULL,
  `sukurimo_data` date NOT NULL DEFAULT current_timestamp(),
  `aukstis` decimal(5,2) DEFAULT NULL,
  `svoris` decimal(5,2) DEFAULT NULL,
  `lytis` enum('Vyras','Moteris') DEFAULT NULL,
  `gimimo_data` date DEFAULT NULL,
  `tikslas` enum('Numesti svorį','Išlaikyti svorį','Priaugti svorį') DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `naudotojo_filtruoti_produktai`
--

CREATE TABLE `naudotojo_filtruoti_produktai` (
  `id_naudotojas` int(6) NOT NULL,
  `id_produktas` int(6) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `patiekalai_is_produktu`
--

CREATE TABLE `patiekalai_is_produktu` (
  `id_patiekalas` int(6) NOT NULL,
  `id_produktas` int(6) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `patiekalas`
--

CREATE TABLE `patiekalas` (
  `id_patiekalas` int(6) NOT NULL,
  `patiekalo_pavadinimas` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `maisto_produktas`
--
ALTER TABLE `maisto_produktas`
  ADD PRIMARY KEY (`id_produktas`),
  ADD UNIQUE KEY `UQ_produkto_pavadinimas` (`produkto_pavadinimas`);

--
-- Indexes for table `naudotojas`
--
ALTER TABLE `naudotojas`
  ADD PRIMARY KEY (`id_naudotojas`);

--
-- Indexes for table `naudotojo_filtruoti_produktai`
--
ALTER TABLE `naudotojo_filtruoti_produktai`
  ADD PRIMARY KEY (`id_naudotojas`,`id_produktas`),
  ADD KEY `nfp_id_produktas_fk` (`id_produktas`);

--
-- Indexes for table `patiekalai_is_produktu`
--
ALTER TABLE `patiekalai_is_produktu`
  ADD PRIMARY KEY (`id_patiekalas`,`id_produktas`),
  ADD KEY `pip_id_produktas_fk` (`id_produktas`);

--
-- Indexes for table `patiekalas`
--
ALTER TABLE `patiekalas`
  ADD PRIMARY KEY (`id_patiekalas`),
  ADD UNIQUE KEY `UQ_produkto_pavadinimas` (`patiekalo_pavadinimas`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `maisto_produktas`
--
ALTER TABLE `maisto_produktas`
  MODIFY `id_produktas` int(6) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT for table `naudotojas`
--
ALTER TABLE `naudotojas`
  MODIFY `id_naudotojas` int(6) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT for table `patiekalas`
--
ALTER TABLE `patiekalas`
  MODIFY `id_patiekalas` int(6) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `naudotojo_filtruoti_produktai`
--
ALTER TABLE `naudotojo_filtruoti_produktai`
  ADD CONSTRAINT `nfp_id_naudotojas_fk` FOREIGN KEY (`id_naudotojas`) REFERENCES `naudotojas` (`id_naudotojas`),
  ADD CONSTRAINT `nfp_id_produktas_fk` FOREIGN KEY (`id_produktas`) REFERENCES `maisto_produktas` (`id_produktas`);

--
-- Constraints for table `patiekalai_is_produktu`
--
ALTER TABLE `patiekalai_is_produktu`
  ADD CONSTRAINT `pip_id_patiekalas_fk` FOREIGN KEY (`id_patiekalas`) REFERENCES `patiekalas` (`id_patiekalas`),
  ADD CONSTRAINT `pip_id_produktas_fk` FOREIGN KEY (`id_produktas`) REFERENCES `maisto_produktas` (`id_produktas`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
