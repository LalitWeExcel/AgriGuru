import { faAd } from "@fortawesome/free-solid-svg-icons";

export interface DashBoard {
  data: TotalSocitiesModel;
  response: string;
  status: boolean;
  message: string;
}

export class TotalSocitiesModel {
  totalNewSocieties: number = 0;
  totalBackLogSocieties: number = 0;
  workingSociety: number = 0
  notWorkingSociety: number = 0;
  defunctSocieties: number = 0;
  underARCS: number = 0;
  underInspector: number = 0;
  deemedApproval: number = 0;
  hearingSocieties: number = 0;
  rejected: number = 0;
  totalApproved: number = 0;
  backlogSociety: number = 0;
}


export class NewsFeedModel {
  id: number = 0;
  title: string="";
  description: string = "";
  isActive: boolean = false;
  creditedOn: Date = new Date();
  expiredOn: Date = new Date();
  creditedBy: number = 0;
}

export interface NewsFeeds {
  data: NewsFeedModel[];
  recordsCount: number ;
}


