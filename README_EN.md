[日本語 README](README.md)

# Project / Executable List

The CraftBandMesh series is a collection of applications that support
the **design, calculation, verification, and documentation**
of baskets made from band-shaped materials such as paper bands and craft tapes.

The official website hosts a large number of **free project data**
created with this software, published together with photos of the finished works.
These datasets are positioned not merely as samples,
but as a **reference database of real-world designs and verified constructions**.


## CraftBandMesh / CraftBandMesh.exe
An application for calculating basket sizes using vertical/horizontal (including oval),
round, and arc-shaped bases with woven sides.  
By selecting band types, weaving methods, and accessories from a database,
you can easily create original craft band or paper band recipes.  
The preview feature helps verify band spacing and side weaving balance.

## CraftBandSqare45 / CraftBandSqare45.exe
An application for calculating sizes when raising a vertically and horizontally woven base
at a 45-degree angle, as used in Nordic weaving and diagonal patterns.  
Includes a preview function for easy layout confirmation.  
Images are generated at actual scale and can be printed as templates.  
Supports OriColor Weave.

## CraftBandKnot / CraftBandKnot.exe
An application for calculating sizes for Yotsudatami
(stone pavement / knot / double-band) weaving.  
Each band type has its own gauge, and required length and unit size can be calculated using
measured values, calculated coefficients, or default coefficients.  
Preview images reflect unit shape (left/right) and band colors.

## CraftBandSquare / CraftBandSquare.exe
An application for calculating sizes when raising a vertically and horizontally woven base directly.  
Specifying band spacing produces a square weave.  
You can insert vertical, horizontal, or diagonal bands,
adjust band width and color individually,
and design patterns by previewing crossing rules.

## CraftBandHexagon / CraftBandHexagon.exe
An application for designing baskets woven in three directions at 60-degree intervals.  
Calculations can be based on band width, hexagon size, or adjacent triangle size.  
Side hexagon size can be adjusted according to the raising position.  
Useful for creating hexagon templates and drawing right/left twill or three-axis patterns.

## CbMesh / CbMesh.exe
A launcher for the series applications.  
By associating file extensions, it launches the appropriate application among the executables.

## CraftBand / CraftBand.dll
A shared library providing configuration databases and common editing forms.


# Features
* Design baskets made from band materials
* Register band types, weaving methods, and accessories as configuration data
* Share the same configuration data across all applications in the series
* Adjust band width and count while checking final dimensions
* Determine sizes based on available band length
* Output color-based cut lists
* Simulate combinations of band color, width, and crossing patterns
* Some applications support 3D preview of the finished shape
* Recent versions allow storing not only the final design  
  but also the **manufacturing process** as part of the project data
* A large collection of free project data with production photos is available on the official website,
  serving as a reference database based on real manufacturing results


# CraftBandMesh XML Format (.cbmesh)

The CraftBandMesh XML Format is a unified XML-based data format
used across the CraftBandMesh series.

It extends and integrates data previously stored as `.xml` files.  
Starting from **Version 1.9**, `.cbmesh` is the officially recommended extension.  
Older `.xml` files remain fully compatible.

## File Extensions
- `.cbmesh` – Used for both project data and master configuration data (recommended)
- `.CBMESH` – Conventionally used to distinguish master data in documentation (functionally identical)

## Data Structure
The format uses an XML DataSet structure defined by an XSD schema.
- Master Configuration Data: `<dstMasterTables>`
- Project Data: `<dstDataTables>`

Internally, all formats are processed via `ReadXml()`,
allowing all extensions and cases to be handled identically.


# Requirements
* Microsoft .NET Runtime 6.0
* Microsoft Windows Desktop Runtime 6.0


# Installation
You can place the extracted binary files in any directory.
Each `.exe` can be launched directly.


# Quick Start

### 1. Download
Download the latest version from the GitHub Releases page.

[Download latest release](releases/latest)

### 2. Installation
This software is a Windows desktop application.  
Run the downloaded installer (`setup.exe`) to install the software.

.NET 6.0 or later is required.
If it is not installed, you will be guided to install it
from the official Microsoft page on first launch.

### 3. Open Your First Project
1. Launch **CbMesh.exe** (the launcher for the series)
2. Download any free sample data published on the official website
3. Open the downloaded data and check the preview
4. Try changing values such as dimensions or number of bands
   and observe how the result changes

The sample data represents real, buildable basket designs
and can be used as practical references for learning and experimentation.



# Current Binary Version
- Installer         1.9.1
- CraftBand.dll     1.9.1.0
- CraftBandMesh     1.9.1.0
- CraftBandSqare45  1.6.1.0
- CraftBandKnot     1.5.1.0
- CraftBandSquare   1.4.1.0
- CraftBandHexagon  1.1.1.0
- CbMesh            1.0.1.0


# Usage
* Series overview and documentation  
  https://labo.com/CraftBand/craftbandmesh-series/
* CraftBandMesh  
  https://labo.com/CraftBand/CraftbandMesh/
* CraftBandSqare45  
  https://labo.com/CraftBand/CraftBandSquare45/
* CraftBandKnot  
  https://labo.com/CraftBand/CraftBandKnot/
* CraftBandSquare  
  https://labo.com/CraftBand/CraftBandSquare/
* CraftBandHexagon  
  https://labo.com/CraftBand/CraftBandHexagon/
* CbMesh  
  https://labo.com/CraftBand/CbMesh/
* Samples and free data  
  https://labo.com/CraftBand/sitemap/


# Author
* Miho Harusawa (CraftBandLabo)
* E-mail: haru@labo.com


# License
CraftBand Series is released under the MIT License.


## Project Contributors Wanted

Contributions to this project are welcome, including feature development,
derivative tools, documentation, and experimental use cases.

Independent use, extension, and interpretation of the standard format (`.cbmesh`)
and shared libraries are explicitly encouraged.
Feel free to build specialized or experimental projects on top of this series.
