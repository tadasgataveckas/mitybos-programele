-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Mar 08, 2024 at 09:19 PM
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
-- Database: `food_db`
--

-- --------------------------------------------------------

--
-- Table structure for table `allergies_from_products`
--

CREATE TABLE `allergies_from_products` (
  `id_allergy` int(6) NOT NULL,
  `id_product` int(6) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `allergy`
--

CREATE TABLE `allergy` (
  `id_allergy` int(6) NOT NULL,
  `allergy_name` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `meal`
--

CREATE TABLE `meal` (
  `id_meal` int(6) NOT NULL,
  `meal_name` varchar(50) NOT NULL,
  `meal_type` enum('Breakfast','Lunch - Dinner','Snack') NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `meals_from_products`
--

CREATE TABLE `meals_from_products` (
  `id_meal` int(6) NOT NULL,
  `id_product` int(6) NOT NULL,
  `amount` decimal(8,2) NOT NULL DEFAULT 0.00
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `product`
--

CREATE TABLE `product` (
  `id_product` int(6) NOT NULL,
  `product_name` varchar(50) NOT NULL,
  `kcal` decimal(7,2) UNSIGNED NOT NULL DEFAULT 0.00,
  `protein` decimal(7,2) UNSIGNED NOT NULL DEFAULT 0.00,
  `carbohydrates` decimal(7,2) UNSIGNED NOT NULL DEFAULT 0.00,
  `fat` decimal(7,2) UNSIGNED NOT NULL DEFAULT 0.00
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `user`
--

CREATE TABLE `user` (
  `id_user` varchar(100) NOT NULL,
  `email` varchar(100) NOT NULL,
  `username` varchar(100) NOT NULL,
  `password` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `user_data`
--

CREATE TABLE `user_data` (
  `id_user` varchar(100) NOT NULL,
  `height` decimal(5,2) UNSIGNED NOT NULL,
  `weight` decimal(5,2) UNSIGNED NOT NULL,
  `gender` enum('Male','Female','Other') NOT NULL,
  `goal` enum('Lose weight','Maintain weight','Gain weight','Gain muscle') NOT NULL,
  `physical_activity` int(1) UNSIGNED NOT NULL,
  `date_of_birth` date NOT NULL,
  `last_updated_date` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `user_filtered_products`
--

CREATE TABLE `user_filtered_products` (
  `id_user` varchar(100) NOT NULL,
  `id_product` int(6) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `user_selected_allergies`
--

CREATE TABLE `user_selected_allergies` (
  `id_user` varchar(100) NOT NULL,
  `id_allergy` int(6) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `allergies_from_products`
--
ALTER TABLE `allergies_from_products`
  ADD PRIMARY KEY (`id_allergy`,`id_product`),
  ADD KEY `afp_id_product_fk` (`id_product`);

--
-- Indexes for table `allergy`
--
ALTER TABLE `allergy`
  ADD PRIMARY KEY (`id_allergy`),
  ADD UNIQUE KEY `allergy_name` (`allergy_name`);

--
-- Indexes for table `meal`
--
ALTER TABLE `meal`
  ADD PRIMARY KEY (`id_meal`),
  ADD UNIQUE KEY `meal_name` (`meal_name`);

--
-- Indexes for table `meals_from_products`
--
ALTER TABLE `meals_from_products`
  ADD PRIMARY KEY (`id_meal`,`id_product`),
  ADD KEY `mfp_id_product_fk` (`id_product`);

--
-- Indexes for table `product`
--
ALTER TABLE `product`
  ADD PRIMARY KEY (`id_product`),
  ADD UNIQUE KEY `product_name` (`product_name`);

--
-- Indexes for table `user`
--
ALTER TABLE `user`
  ADD PRIMARY KEY (`id_user`),
  ADD UNIQUE KEY `email` (`email`),
  ADD UNIQUE KEY `username` (`username`);

--
-- Indexes for table `user_data`
--
ALTER TABLE `user_data`
  ADD PRIMARY KEY (`id_user`);

--
-- Indexes for table `user_filtered_products`
--
ALTER TABLE `user_filtered_products`
  ADD PRIMARY KEY (`id_user`,`id_product`),
  ADD KEY `ufp_id_product_fk` (`id_product`);

--
-- Indexes for table `user_selected_allergies`
--
ALTER TABLE `user_selected_allergies`
  ADD PRIMARY KEY (`id_user`,`id_allergy`),
  ADD KEY `usa_id_allergy_fk` (`id_allergy`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `allergy`
--
ALTER TABLE `allergy`
  MODIFY `id_allergy` int(6) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `meal`
--
ALTER TABLE `meal`
  MODIFY `id_meal` int(6) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=67;

--
-- AUTO_INCREMENT for table `product`
--
ALTER TABLE `product`
  MODIFY `id_product` int(6) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=338;


--
-- Constraints for dumped tables
--

--
-- Constraints for table `allergies_from_products`
--
ALTER TABLE `allergies_from_products`
  ADD CONSTRAINT `afp_id_allergy_fk` FOREIGN KEY (`id_allergy`) REFERENCES `allergy` (`id_allergy`),
  ADD CONSTRAINT `afp_id_product_fk` FOREIGN KEY (`id_product`) REFERENCES `product` (`id_product`);

--
-- Constraints for table `meals_from_products`
--
ALTER TABLE `meals_from_products`
  ADD CONSTRAINT `mfp_id_meals_fk` FOREIGN KEY (`id_meal`) REFERENCES `meal` (`id_meal`),
  ADD CONSTRAINT `mfp_id_product_fk` FOREIGN KEY (`id_product`) REFERENCES `product` (`id_product`);

--
-- Constraints for table `user_data`
--
ALTER TABLE `user_data`
  ADD CONSTRAINT `ud_id_user_fk` FOREIGN KEY (`id_user`) REFERENCES `user` (`id_user`);

--
-- Constraints for table `user_filtered_products`
--
ALTER TABLE `user_filtered_products`
  ADD CONSTRAINT `ufp_id_product_fk` FOREIGN KEY (`id_product`) REFERENCES `product` (`id_product`),
  ADD CONSTRAINT `ufp_id_user_fk` FOREIGN KEY (`id_user`) REFERENCES `user` (`id_user`);

--
-- Constraints for table `user_selected_allergies`
--
ALTER TABLE `user_selected_allergies`
  ADD CONSTRAINT `usa_id_allergy_fk` FOREIGN KEY (`id_allergy`) REFERENCES `allergy` (`id_allergy`),
  ADD CONSTRAINT `usa_id_user_fk` FOREIGN KEY (`id_user`) REFERENCES `user` (`id_user`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
