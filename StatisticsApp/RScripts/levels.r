args = commandArgs(trailingOnly = TRUE)

data <- read.table(args[1],sep=",",header = TRUE, 
                   stringsAsFactors = TRUE)
variable <- as.numeric(args[2])
data[,variable] <- factor(data[,variable])
output <- c()
for(level in levels(data[,variable])){
  output <- c(output, level)
}
cat(paste(output, collapse = ","))