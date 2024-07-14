# Solvia Network Event Logger Service

![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)
![.NET Core](https://github.com/itschris/SolviaNetworkEventLoggerSvc/workflows/.NET%20Core/badge.svg)

## Overview

Solvia Network Event Logger Service is a Windows Service that monitors network connect and disconnect events and logs them to CSV files. Additionally, it pings specified IP addresses every second and logs the results to date-stamped files. This service is built using .NET Core and designed to run in the background on Windows systems.

## Features

- Logs network connect and disconnect events.
- Pings specified IP addresses (e.g., 1.1.1.1 and 192.168.147.1) every second and logs the results.
- Logs are written to date-stamped CSV files for easy analysis.
- Built as a Windows Service for continuous background operation.

## Prerequisites

- .NET Core SDK
- Windows operating system (for running as a Windows Service)

## Installation

### Step 1: Clone the Repository

```bash
git clone https://github.com/itschris/SolviaNetworkEventLoggerSvc.git
cd SolviaNetworkEventLoggerSvc
