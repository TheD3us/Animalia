export interface Event {
  Id: number;
  UserId: number;
  Title: string;
  DateTime: string;
  Location: string;
  Notes: string;
  MaxParticipants?: number;
}
