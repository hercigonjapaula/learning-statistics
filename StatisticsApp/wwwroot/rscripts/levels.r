args = commandArgs(trailingOnly = TRUE)

data <- read.table(args[1],sep=";",header = TRUE, 
                   stringsAsFactors = TRUE)
variable <- as.numeric(args[2])
data[,variable] <- factor(data[,variable])
print(levels(data[,variable]))