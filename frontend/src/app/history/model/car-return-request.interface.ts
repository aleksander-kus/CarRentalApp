export interface CarReturnRequest {
  rentId: string;
  historyEntryId: string;
  userEmail: string;
  rentDate: Date;
  returnDate: Date;
  carCondition: string;
  odometerValue: number;
  photoFileId: string;
  pdfFileId: string;
}
