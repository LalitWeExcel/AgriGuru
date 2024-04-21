export class ByeLwasRequestListModel { 
  byeLawsRequestId = 0;
  nameOfCeo = '';
  dateOfNoticeForGBM = '';
  dateOfGBMAmendmentsSec = '';
  noOfDaysOfNoticeForGBM = '';
  totalNoOfMembersMSCS = '';
  isQuorumSpecified = '';
  quorumRequiredForMeeting = '';
  noOfMemberPresentInMeeting = '';
  noOfMembersVotedInMeeting = '';
  wasTheGBMMeetingAdjourned = '';
  noOfMemVotedInTheFavorOfProposedAmendment = '';
  additionalInfo = '';
  amemndmentInByLawRelatedTo = '';
  existingByeLaw = '';
  proposedByeLaw = '';
  reasonForAmendment = '';
  resolutionOfGMDoc = '';
  resolutionOfAmendmentDoc = '';
  reasonForAmendmentDoc = '';
  amendedTextOfTheBylawsDoc = '';
  certificationOfAmendmentBylaws = '';
  entryDate = '';
  updateDate = '';
  entryBy = 0;
  updateBy = 0;
  status = 0;
}

export interface ByeLawsRequest {
  data: ByeLwasRequestListModel[];
  recordsCount: number;
}
