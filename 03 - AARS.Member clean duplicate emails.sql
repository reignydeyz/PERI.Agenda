delete Member
where email in
(select email from Member
where email is not null and email != ''
group by [Email] having count(*) > 1)