import { Component, input } from '@angular/core';
import { Member } from '../../_models/member';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-member-card',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './member-card.component.html',
  styleUrl: './member-card.component.css'
})
export class MemberCardComponent {
  member = input.required<Member>();
  likedMembers = new Set<number>();

  toggleLike(memberId: number) {
    if (this.likedMembers.has(memberId)) {
      this.likedMembers.delete(memberId);
    } else {
      this.likedMembers.add(memberId);
    }
  }

  isLiked(memberId: number): boolean {
    return this.likedMembers.has(memberId);
  }
}
